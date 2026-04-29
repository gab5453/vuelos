<script setup>
import { ref, computed, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'
import { getVuelo, getAsientos } from '../services/vuelos.service'
import { getClientes, crearCliente, crearPasajero } from '../services/crm.service'
import { getMetodosPago, registrarMetodoPago } from '../services/pagos.service'
import { crearReserva, emitirFactura, emitirBoleto, cambiarEstadoReserva, registrarEquipaje } from '../services/ventas.service'

const router = useRouter()
const flightStore = useFlightStore()
const authStore = useAuthStore()

const selectedFlight = ref(flightStore.selectedFlight || null)

// Si no hay vuelo seleccionado, redirigir al inicio
onMounted(() => {
  if (!selectedFlight.value) {
    router.push('/')
  }
})

// Formularios
const billingForm = ref({
  firstName: '',
  lastName: '',
  email: '',
  documentId: ''
})

const paymentForm = ref({
  cardNumber: '',
  expiry: '',
  cvv: '',
  cardName: ''
})

// MOCK DE ASIENTOS
const numPassengers = computed(() => flightStore.searchParams?.passengers || 1)
const selectedSeats = ref([])
const seats = ref([])

// Metodos de Pago
const savedMethods = ref([])
const selectedMethodId = ref(null) // null = nueva tarjeta
const shouldSaveCard = ref(false)
const isLoadingMethods = ref(false)

// Formulario de los Pasajeros eliminado según requerimiento


// MOCK DE ASIENTOS ELIMINADO

const isLoadingSeats = ref(true)

const loadSavedMethods = async () => {
  if (!authStore.user) return
  try {
    isLoadingMethods.value = true
    // Primero necesitamos el id_cliente real del CRM
    const clients = await getClientes({ identificacion: authStore.user.identificacion })
    if (clients && clients.length > 0) {
      const id_cliente = clients[0].id_cliente
      const methods = await getMetodosPago(id_cliente)
      savedMethods.value = methods || []
      if (savedMethods.value.length > 0) {
        selectedMethodId.value = savedMethods.value[0].id_metodo
      }
    }
  } catch (err) {
    console.warn('Error al cargar métodos de pago guardados.')
  } finally {
    isLoadingMethods.value = false
  }
}

onMounted(async () => {
  try {
    isLoadingSeats.value = true
    
    // Pre-llenar datos del usuario si esta autenticado
    if (authStore.user) {
      billingForm.value.firstName = authStore.user.nombre || ''
      billingForm.value.email = authStore.user.email || authStore.user.correo || ''
      billingForm.value.documentId = authStore.user.identificacion || ''
    }

    // 2. Obtener asientos reales
    const asientosData = await getAsientos(selectedFlight.value.id_vuelo)
    
    // 3. Cargar metodos de pago si el cliente ya existe
    if (authStore.user?.identificacion) {
      loadSavedMethods()
    }

    if (asientosData && asientosData.length > 0) {
      // Agrupar el array plano en filas de 6 para el mapa visual
      const groupedSeats = []
      for (let i = 0; i < asientosData.length; i += 6) {
        groupedSeats.push(asientosData.slice(i, i + 6))
      }
      seats.value = groupedSeats
    }
  } catch (error) {
    console.error('Error al cargar asientos:', error)
  } finally {
    isLoadingSeats.value = false
  }
})

const selectSeat = (seat) => {
  if (seat.ocupado) return

  const isSelected = selectedSeats.value.some(s => s.id_asiento === seat.id_asiento)
  
  if (isSelected) {
    // Deseleccionar
    selectedSeats.value = selectedSeats.value.filter(s => s.id_asiento !== seat.id_asiento)
  } else {
    // Seleccionar si no excede N
    if (selectedSeats.value.length < numPassengers.value) {
      selectedSeats.value.push(seat)
    } else {
      alert(`Solo necesitas seleccionar ${numPassengers.value} asiento(s).`)
    }
  }
}

// Equipaje Extra
const baggageSelections = ref({}) // key: id_asiento, value: cantidad (0 a 2)
const BAGGAGE_PRICE = 50

const incrementBaggage = (id_asiento) => {
  const current = baggageSelections.value[id_asiento] || 0
  if (current < 2) baggageSelections.value[id_asiento] = current + 1
}

const decrementBaggage = (id_asiento) => {
  const current = baggageSelections.value[id_asiento] || 0
  if (current > 0) baggageSelections.value[id_asiento] = current - 1
}

const getBaggageCount = (id_asiento) => {
  return baggageSelections.value[id_asiento] || 0
}

const totalBaggagePrice = computed(() => {
  let total = 0
  for (const seat of selectedSeats.value) {
    total += getBaggageCount(seat.id_asiento) * BAGGAGE_PRICE
  }
  return total
})

const totalPrice = computed(() => {
  const baseFlightsPrice = selectedFlight.value.price * numPassengers.value
  const taxes = 45 * numPassengers.value
  const extraSeatsPrice = selectedSeats.value.reduce((sum, seat) => sum + seat.precio_asiento_extra, 0)
  return baseFlightsPrice + taxes + extraSeatsPrice + totalBaggagePrice.value
})

const isProcessing = ref(false)
const VuelosError = ref(null)
const VuelosSuccessMessage = ref(null)

const submitVuelos = async () => {
  if (selectedSeats.value.length !== numPassengers.value) {
    alert(`Por favor selecciona exactamente ${numPassengers.value} asiento(s) en el mapa.`)
    return
  }

  // Validar Facturacion (Cliente)
  if (!billingForm.value.firstName || !billingForm.value.lastName || !billingForm.value.email || !billingForm.value.documentId) {
    alert('Completa los datos del titular para facturación.')
    return
  }


  isProcessing.value = true
  VuelosError.value = null

  try {
    // 1. Verificar si el cliente ya existe o crearlo
    let id_cliente = null
    const existingClients = await getClientes({ 
      identificacion: billingForm.value.documentId,
      correo: billingForm.value.email 
    })

    if (existingClients && existingClients.length > 0) {
      id_cliente = existingClients[0].id_cliente
    } else {
      const nuevoCliente = await crearCliente({
        tipo_identificacion: 'CEDULA',
        numero_identificacion: billingForm.value.documentId,
        nombres: billingForm.value.firstName,
        apellidos: billingForm.value.lastName,
        correo: billingForm.value.email,
        telefono: '0999999999', // Campo obligatorio segun contrato
        direccion: 'Calle Principal 123', // Campo obligatorio segun contrato
        id_ciudad_residencia: 1, // Mock ID
        id_pais_nacionalidad: 1, // Mock ID
        estado: 'ACT'
      })
      id_cliente = nuevoCliente?.id_cliente
    }

    // 2. Crear Pasajero (Payload segun seccion 7.1 del contrato)
    const pasajeroData = await crearPasajero({
      nombre_pasajero: billingForm.value.firstName,
      apellido_pasajero: billingForm.value.lastName,
      tipo_documento_pasajero: 'CEDULA',
      numero_documento_pasajero: billingForm.value.documentId,
      id_cliente: id_cliente
    })
    const id_pasajero = pasajeroData?.id_pasajero || 1

    const now = new Date().toISOString()
    const reservations = []
    
    // 3. Iterar por cada asiento para crear las reservas a nombre del mismo pasajero/cliente
    for (let i = 0; i < numPassengers.value; i++) {
      const seat = selectedSeats.value[i]

      // Crear reserva
      const reservaData = await crearReserva({
        id_cliente: id_cliente,
        id_pasajero: id_pasajero,
        id_vuelo: selectedFlight.value.id_vuelo,
        id_asiento: seat.id_asiento,
        fecha_inicio: now,
        fecha_fin: now, 
        subtotal_reserva: selectedFlight.value.price + seat.precio_asiento_extra,
        valor_iva: 45,
        total_reserva: selectedFlight.value.price + seat.precio_asiento_extra + 45,
        origen_canal_reserva: 'Vuelos', // Segun contrato 8.1
        estado_reserva: 'PEN'
      })
      
      reservations.push({
        id_reserva: reservaData?.id_reserva || (100 + i),
        seat: seat
      })
    }

    // 2.5 Gestion de Metodo de Pago
    let final_id_metodo = selectedMethodId.value

    if (!final_id_metodo) {
      // Si es una tarjeta nueva y el usuario marco "guardar"
      if (shouldSaveCard.value) {
        const nuevoMetodo = await registrarMetodoPago(id_cliente, {
          tipo_metodo: 'TARJETA',
          proveedor: paymentForm.value.cardNumber.startsWith('4') ? 'VISA' : 'MASTERCARD',
          numero_enmascarado: `**** **** **** ${paymentForm.value.cardNumber.slice(-4)}`,
          titular: paymentForm.value.cardName,
          fecha_expiracion: paymentForm.value.expiry,
          estado: 'ACT'
        })
        final_id_metodo = nuevoMetodo?.id_metodo || 1
      } else {
        final_id_metodo = 1 // ID por defecto para transacciones anonimas
      }
    }

    // 3. Crear 1 Factura global
    const facturaData = await emitirFactura({
      id_reserva: reservations[0].id_reserva,
      id_cliente: id_cliente,
      id_metodo: final_id_metodo,
      subtotal: totalPrice.value - (45 * numPassengers.value),
      valor_iva: 45 * numPassengers.value, 
      total: totalPrice.value,
      estado: 'APR'
    })
    const id_factura = facturaData?.id_factura || 1

    // 4. Emitir Boletos, registrar equipaje y actualizar reservas
    for (const res of reservations) {
      const baggageCount = getBaggageCount(res.seat.id_asiento)
      const seatBaggagePrice = baggageCount * BAGGAGE_PRICE

      const boletoData = await emitirBoleto({
        id_reserva: res.id_reserva,
        id_vuelo: selectedFlight.value.id_vuelo,
        id_asiento: res.seat.id_asiento,
        id_factura: id_factura,
        clase: res.seat.clase,
        precio_vuelo_base: selectedFlight.value.price,
        precio_asiento_extra: res.seat.precio_asiento_extra,
        impuestos_boleto: 45,
        cargo_equipaje: seatBaggagePrice,
        precio_final: selectedFlight.value.price + res.seat.precio_asiento_extra + 45 + seatBaggagePrice,
        estado_boleto: 'ACTIVO'
      })
      
      const id_boleto = boletoData?.id_boleto || 1
      
      // Registrar equipaje de bodega (si lo hay)
      for (let i = 0; i < baggageCount; i++) {
        await registrarEquipaje(id_boleto, {
          id_boleto: id_boleto,
          tipo: 'BODEGA',
          peso_kg: 23,
          precio_extra: BAGGAGE_PRICE
        })
      }

      // Registrar equipaje de mano gratuito por defecto (10kg max según contrato)
      await registrarEquipaje(id_boleto, {
        id_boleto: id_boleto,
        tipo: 'MANO',
        peso_kg: 10,
        precio_extra: 0
      })
      
      await cambiarEstadoReserva(res.id_reserva, 'EMI')
    }

    VuelosSuccessMessage.value = `¡Reserva exitosa para ${numPassengers.value} pasajero(s)! Su pago fue aprobado.`
    
    setTimeout(() => {
      router.push('/')
    }, 4500)

  } catch (error) {
    console.error('Error al procesar reserva múltiple:', error)
    VuelosError.value = error.response?.data?.mensaje || 'Error procesando la transacción de boletos.'
  } finally {
    isProcessing.value = false
  }
}
</script>

<template>
  <div class="Vuelos-container">
    <div class="header">
      <div class="container">
        <h1>Completa tu Reserva</h1>
        <p>Estás a un paso de tu próximo destino</p>
      </div>
    </div>

    <div class="content-wrapper container">
      <!-- Sección de Formularios -->
      <div class="main-content">
        
        <!-- Selección de Asiento -->
        <section class="Vuelos-section">
          <h2>1. Selección de Asiento</h2>
          <div class="seat-map-container">
            <div class="plane-fuselage">
              <div class="row" v-for="(row, index) in seats" :key="index">
                <div class="seat-group left">
                  <div 
                    v-for="seat in row.slice(0, 3)" 
                    :key="seat.id_asiento"
                    class="seat"
                    :class="{ 
                      'occupied': seat.ocupado, 
                      'selected': selectedSeats.some(s => s.id_asiento === seat.id_asiento),
                      'first-class': seat.clase === 'EJECUTIVA'
                    }"
                    @click="selectSeat(seat)"
                    :title="`Asiento ${seat.numero} - ${seat.clase} (+$${seat.precio_asiento_extra})`"
                  >
                    {{ seat.numero }}
                  </div>
                </div>
                <div class="aisle"></div>
                <div class="seat-group right">
                  <div 
                    v-for="seat in row.slice(3, 6)" 
                    :key="seat.id_asiento"
                    class="seat"
                    :class="{ 
                      'occupied': seat.ocupado, 
                      'selected': selectedSeats.some(s => s.id_asiento === seat.id_asiento),
                      'first-class': seat.clase === 'EJECUTIVA'
                    }"
                    @click="selectSeat(seat)"
                    :title="`Asiento ${seat.numero} - ${seat.clase} (+$${seat.precio_asiento_extra})`"
                  >
                    {{ seat.numero }}
                  </div>
                </div>
              </div>
            </div>
            
            <div class="seat-legend">
              <div class="legend-item"><div class="seat-box available"></div> Disponible</div>
              <div class="legend-item"><div class="seat-box occupied"></div> Ocupado</div>
              <div class="legend-item"><div class="seat-box selected"></div> Seleccionado</div>
              <div class="legend-item"><div class="seat-box first-class"></div> Ejecutiva</div>
            </div>
          </div>
        </section>

        <!-- Selección de Equipaje -->
        <section class="Vuelos-section">
          <h2>2. Equipaje Extra</h2>
          <p style="color: #64748b; margin-bottom: 20px; font-size: 0.95rem;">Tu boleto ya incluye 1 maleta de mano (hasta 10kg). Puedes agregar maletas de bodega (hasta 23kg) por ${{ BAGGAGE_PRICE }} cada una.</p>
          
          <div v-for="seat in selectedSeats" :key="seat.id_asiento" class="baggage-card">
            <div class="baggage-info">
              <h3>Asiento {{ seat.numero }}</h3>
              <p>Equipaje de Bodega (23kg)</p>
            </div>
            <div class="baggage-controls">
              <button type="button" class="btn-bag" @click="decrementBaggage(seat.id_asiento)" :disabled="getBaggageCount(seat.id_asiento) === 0">-</button>
              <span class="bag-count">{{ getBaggageCount(seat.id_asiento) }}</span>
              <button type="button" class="btn-bag" @click="incrementBaggage(seat.id_asiento)" :disabled="getBaggageCount(seat.id_asiento) >= 2">+</button>
            </div>
          </div>
        </section>


        <!-- Formulario Facturación -->
        <section class="Vuelos-section">
          <h2>3. Datos de Facturación (Titular)</h2>
          <div class="form-grid">
            <div class="input-group">
              <label>Nombre</label>
              <input type="text" v-model="billingForm.firstName" placeholder="Ej. Juan" required>
            </div>
            <div class="input-group">
              <label>Apellidos</label>
              <input type="text" v-model="billingForm.lastName" placeholder="Ej. Pérez" required>
            </div>
            <div class="input-group">
              <label>Correo Electrónico</label>
              <input type="email" v-model="billingForm.email" placeholder="juan@email.com" required>
            </div>
            <div class="input-group">
              <label>Pasaporte / DNI / RUC</label>
              <input type="text" v-model="billingForm.documentId" placeholder="Número de documento fiscal" required>
            </div>
          </div>
        </section>

        <!-- Formulario Pago -->
        <section class="Vuelos-section">
          <h2>4. Método de Pago</h2>
          
          <div v-if="savedMethods.length > 0" class="saved-methods-container">
            <p class="section-hint">Selecciona una tarjeta guardada o usa una nueva:</p>
            <div class="methods-list">
              <label 
                v-for="method in savedMethods" 
                :key="method.id_metodo" 
                class="method-item"
                :class="{ selected: selectedMethodId === method.id_metodo }"
              >
                <input 
                  type="radio" 
                  v-model="selectedMethodId" 
                  :value="method.id_metodo"
                >
                <span class="card-icon">💳</span>
                <div class="method-details">
                  <span class="card-number">{{ method.numero_enmascarado }}</span>
                  <span class="card-type">{{ method.proveedor }}</span>
                </div>
              </label>
              
              <label class="method-item" :class="{ selected: selectedMethodId === null }">
                <input type="radio" v-model="selectedMethodId" :value="null">
                <span class="card-icon">➕</span>
                <span class="method-label">Usar una nueva tarjeta</span>
              </label>
            </div>
          </div>

          <div v-if="selectedMethodId === null" class="form-grid fade-in">
            <div class="input-group full-width">
              <label>Titular de la tarjeta</label>
              <input type="text" v-model="paymentForm.cardName" placeholder="Como aparece en la tarjeta" required>
            </div>
            <div class="input-group full-width">
              <label>Número de Tarjeta</label>
              <input type="text" v-model="paymentForm.cardNumber" placeholder="0000 0000 0000 0000" required>
            </div>
            <div class="input-group">
              <label>Fecha de Expiración</label>
              <input type="text" v-model="paymentForm.expiry" placeholder="MM/AA" required>
            </div>
            <div class="input-group">
              <label>CVV</label>
              <input type="password" v-model="paymentForm.cvv" placeholder="123" required maxlength="4">
            </div>
            <div class="input-group full-width save-card-option">
              <label class="checkbox-label">
                <input type="checkbox" v-model="shouldSaveCard">
                Guardar esta tarjeta para futuras compras
              </label>
            </div>
          </div>
          <div v-else class="selected-method-summary fade-in">
            <p>Has seleccionado pagar con tu tarjeta terminada en <strong>{{ savedMethods.find(m => m.id_metodo === selectedMethodId)?.numero_enmascarado.slice(-4) }}</strong>.</p>
          </div>
        </section>
      </div>

      <!-- Resumen de Reserva (Sidebar) -->
      <aside class="summary-sidebar">
        <div class="summary-card">
          <h2>Resumen del Viaje</h2>
          
          <div class="flight-summary">
            <div class="flight-header">
              <span class="airline">{{ selectedFlight.airline }}</span>
              <span class="date">{{ selectedFlight.departureDate }}</span>
            </div>
            
            <div class="flight-route">
              <div class="route-point">
                <strong>{{ selectedFlight.departureTime }}</strong>
                <span>{{ selectedFlight.origin }}</span>
              </div>
              <div class="route-path">
                <span class="duration">{{ selectedFlight.duration }}</span>
                <div class="line">✈️</div>
              </div>
              <div class="route-point text-right">
                <strong>{{ selectedFlight.arrivalTime }}</strong>
                <span>{{ selectedFlight.destination }}</span>
              </div>
            </div>
          </div>

          <div class="price-breakdown">
            <div class="price-row">
              <span>Vuelo Base (x{{ numPassengers }})</span>
              <span>${{ selectedFlight.price * numPassengers }}</span>
            </div>
            <div v-for="seat in selectedSeats" :key="seat.id_asiento" class="price-row">
              <span>Asiento {{ seat.numero }} ({{ seat.clase }})</span>
              <span>+${{ seat.precio_asiento_extra }}</span>
            </div>
            <div class="price-row">
              <span>Impuestos y tasas (x{{ numPassengers }})</span>
              <span>${{ 45 * numPassengers }}</span>
            </div>
            <div v-if="totalBaggagePrice > 0" class="price-row">
              <span>Equipaje extra de bodega</span>
              <span>+${{ totalBaggagePrice }}</span>
            </div>
            <div class="price-row total">
              <span>Total a Pagar</span>
              <span>${{ totalPrice }}</span>
            </div>
          </div>

          <div v-if="VuelosError" class="error-alert fade-in">
            ⚠️ {{ VuelosError }}
          </div>
          <div v-if="VuelosSuccessMessage" class="success-alert fade-in">
            ✅ {{ VuelosSuccessMessage }}
            <p class="redirect-text">Redirigiendo al inicio en breve...</p>
          </div>

          <button class="confirm-btn" @click="submitVuelos" :disabled="isProcessing || VuelosSuccessMessage">
            <span v-if="isProcessing">Procesando transacción... ⏳</span>
            <span v-else>Confirmar Pago y Reservar</span>
          </button>
          <p class="terms">Al confirmar, aceptas nuestros términos y condiciones y políticas de privacidad.</p>
        </div>
      </aside>
    </div>
  </div>
</template>

<style scoped>
.Vuelos-container {
  min-height: 100vh;
  background-color: var(--bg-color);
  padding-bottom: 60px;
}

.header {
  background: var(--primary);
  color: white;
  padding: 40px 20px;
  margin-bottom: 40px;
  box-shadow: var(--shadow-md);
}

.header h1 {
  font-size: 2rem;
  margin-bottom: 8px;
}

.header p {
  opacity: 0.9;
  font-size: 1.1rem;
}

.container {
  max-width: 1100px;
  margin: 0 auto;
}

.content-wrapper {
  display: grid;
  grid-template-columns: 1fr 380px;
  gap: 32px;
  padding: 0 20px;
}

.Vuelos-section {
  background: white;
  border-radius: var(--radius-md);
  padding: 32px;
  margin-bottom: 24px;
  box-shadow: var(--shadow-sm);
  border: 1px solid #e2e8f0;
}

.Vuelos-section h2 {
  font-size: 1.3rem;
  color: var(--text-main);
  margin-bottom: 24px;
  padding-bottom: 12px;
  border-bottom: 1px solid #e2e8f0;
}

.form-grid {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 20px;
}

.input-group {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.input-group.full-width {
  grid-column: 1 / -1;
}

.input-group label {
  font-size: 0.9rem;
  font-weight: 600;
  color: var(--text-muted);
}

.input-group input {
  padding: 12px 16px;
  border: 1px solid #cbd5e1;
  border-radius: 8px;
  font-size: 1rem;
  transition: border-color 0.3s;
}

.input-group input:focus {
  border-color: var(--primary);
  outline: none;
  box-shadow: 0 0 0 3px rgba(99, 102, 241, 0.1);
}

.baggage-card {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 16px;
  background: #f8fafc;
  border-radius: 8px;
  border: 1px solid #e2e8f0;
  margin-bottom: 12px;
}

.baggage-info h3 {
  font-size: 1rem;
  color: #1e293b;
  margin-bottom: 4px;
}

.baggage-info p {
  font-size: 0.85rem;
  color: #64748b;
  margin: 0;
}

.baggage-controls {
  display: flex;
  align-items: center;
  gap: 16px;
}

.btn-bag {
  width: 32px;
  height: 32px;
  border-radius: 50%;
  border: 1px solid #cbd5e1;
  background: white;
  color: #334155;
  font-weight: bold;
  font-size: 1.2rem;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: all 0.2s;
}

.btn-bag:hover:not(:disabled) {
  border-color: var(--primary);
  color: var(--primary);
  background: #eef2ff;
}

.btn-bag:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.bag-count {
  font-weight: 600;
  font-size: 1.1rem;
  min-width: 20px;
  text-align: center;
}

.payment-methods {
  margin-bottom: 24px;
}

.saved-methods-container {
  margin-bottom: 24px;
}

.section-hint {
  font-size: 0.9rem;
  color: #64748b;
  margin-bottom: 16px;
}

.methods-list {
  display: flex;
  flex-direction: column;
  gap: 12px;
}

.method-item {
  display: flex;
  align-items: center;
  gap: 16px;
  padding: 16px;
  border: 2px solid #e2e8f0;
  border-radius: 12px;
  cursor: pointer;
  transition: all 0.2s ease;
}

.method-item:hover {
  background: #f8fafc;
  border-color: #cbd5e1;
}

.method-item.selected {
  border-color: var(--primary);
  background: rgba(99, 102, 241, 0.05);
}

.card-icon {
  font-size: 1.5rem;
}

.method-details {
  display: flex;
  flex-direction: column;
}

.card-number {
  font-weight: 700;
  color: #1e293b;
}

.card-type {
  font-size: 0.75rem;
  color: #64748b;
  text-transform: uppercase;
  font-weight: 600;
}

.method-label {
  font-weight: 600;
  color: #475569;
}

.save-card-option {
  margin-top: 10px;
}

.checkbox-label {
  display: flex;
  align-items: center;
  gap: 10px;
  font-size: 0.95rem;
  color: #475569;
  cursor: pointer;
}

.selected-method-summary {
  padding: 20px;
  background: #f0f9ff;
  border-radius: 12px;
  border: 1px solid #bae6fd;
  color: #0369a1;
  text-align: center;
}

/* Estilos del Mapa de Asientos */
.seat-map-container {
  display: flex;
  flex-direction: column;
  align-items: center;
  background: #f8fafc;
  padding: 32px 0;
  border-radius: 16px;
  margin-top: 16px;
}

.plane-fuselage {
  background: white;
  padding: 30px;
  border-radius: 40px 40px 8px 8px;
  box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1);
  border: 4px solid #cbd5e1;
}

.row {
  display: flex;
  justify-content: space-between;
  margin-bottom: 12px;
}

.seat-group {
  display: flex;
  gap: 8px;
}

.aisle {
  width: 40px;
}

.seat {
  width: 40px;
  height: 40px;
  border-radius: 8px 8px 4px 4px;
  background: #e2e8f0;
  border: 2px solid #cbd5e1;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 0.8rem;
  font-weight: 600;
  color: #64748b;
  cursor: pointer;
  transition: all 0.2s ease;
}

.seat:not(.occupied):hover {
  background: #cbd5e1;
  transform: translateY(-2px);
}

.seat.first-class {
  background: #fde047;
  border-color: #facc15;
  color: #854d0e;
}

.seat.first-class:not(.occupied):hover {
  background: #fef08a;
}

.seat.occupied {
  background: #f1f5f9;
  border-color: #e2e8f0;
  color: #cbd5e1;
  cursor: not-allowed;
  opacity: 0.7;
}

.seat.selected {
  background: var(--primary);
  border-color: var(--primary);
  color: white;
  transform: scale(1.1);
  box-shadow: 0 4px 12px rgba(99, 102, 241, 0.4);
}

.seat-legend {
  display: flex;
  gap: 16px;
  margin-top: 24px;
  flex-wrap: wrap;
  justify-content: center;
}

.legend-item {
  display: flex;
  align-items: center;
  gap: 8px;
  font-size: 0.9rem;
  color: var(--text-muted);
}

.seat-box {
  width: 16px;
  height: 16px;
  border-radius: 4px;
  border: 2px solid #cbd5e1;
}

.seat-box.available { background: #e2e8f0; }
.seat-box.occupied { background: #f1f5f9; border-color: #e2e8f0; }
.seat-box.selected { background: var(--primary); border-color: var(--primary); }
.seat-box.first-class { background: #fde047; border-color: #facc15; }

/* Estilos del Sidebar */
.summary-card {
  background: white;
  border-radius: var(--radius-md);
  padding: 24px;
  box-shadow: var(--shadow-glass);
  border: 1px solid #e2e8f0;
  position: sticky;
  top: 24px;
}

.summary-card h2 {
  font-size: 1.2rem;
  margin-bottom: 20px;
  color: var(--text-main);
}

.flight-summary {
  background: #f8fafc;
  border-radius: 8px;
  padding: 16px;
  margin-bottom: 24px;
  border: 1px solid #e2e8f0;
}

.flight-header {
  display: flex;
  justify-content: space-between;
  margin-bottom: 16px;
  font-size: 0.9rem;
  color: var(--text-muted);
  font-weight: 500;
}

.flight-route {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.route-point {
  display: flex;
  flex-direction: column;
}

.route-point strong {
  font-size: 1.4rem;
  color: var(--text-main);
}

.route-point span {
  font-size: 0.9rem;
  color: var(--text-muted);
}

.text-right {
  text-align: right;
}

.route-path {
  display: flex;
  flex-direction: column;
  align-items: center;
  flex: 1;
  padding: 0 16px;
}

.route-path .duration {
  font-size: 0.8rem;
  color: var(--text-muted);
  margin-bottom: 4px;
}

.route-path .line {
  width: 100%;
  height: 2px;
  background: #cbd5e1;
  position: relative;
  display: flex;
  justify-content: center;
  align-items: center;
  color: var(--primary);
  font-size: 1rem;
}

.price-breakdown {
  margin-bottom: 24px;
}

.price-row {
  display: flex;
  justify-content: space-between;
  margin-bottom: 12px;
  color: var(--text-muted);
  font-size: 0.95rem;
}

.price-row.total {
  margin-top: 16px;
  padding-top: 16px;
  border-top: 1px solid #e2e8f0;
  font-weight: 700;
  color: var(--text-main);
  font-size: 1.2rem;
}

.confirm-btn {
  width: 100%;
  background: var(--accent); /* Color rojo rosado para llamar la atención */
  color: white;
  padding: 16px;
  border-radius: 8px;
  font-size: 1.1rem;
  font-weight: 600;
  border: none;
  cursor: pointer;
  box-shadow: 0 4px 14px 0 rgba(244, 63, 94, 0.39);
  transition: transform 0.2s, box-shadow 0.2s;
  margin-bottom: 12px;
}

.confirm-btn:hover {
  transform: translateY(-2px);
  box-shadow: 0 6px 20px rgba(244, 63, 94, 0.3);
  background: #e11d48;
}

.terms {
  font-size: 0.8rem;
  color: var(--text-muted);
  text-align: center;
}

.error-alert {
  background: #fef2f2;
  color: #ef4444;
  padding: 12px;
  border-radius: 8px;
  margin-bottom: 16px;
  border: 1px solid #fecaca;
  font-size: 0.9rem;
  font-weight: 500;
}

.success-alert {
  background: #f0fdf4;
  color: #166534;
  padding: 16px;
  border-radius: 8px;
  margin-bottom: 16px;
  border: 1px solid #bbf7d0;
  font-weight: 600;
  text-align: center;
}

.redirect-text {
  font-size: 0.8rem;
  color: #15803d;
  margin-top: 4px;
  font-weight: 400;
}

.confirm-btn:disabled {
  opacity: 0.7;
  cursor: not-allowed;
  background: #94a3b8;
  box-shadow: none;
}

.fade-in {
  animation: fadeInAlert 0.4s ease forwards;
}

@keyframes fadeInAlert {
  from { opacity: 0; transform: translateY(-5px); }
  to { opacity: 1; transform: translateY(0); }
}

@media (max-width: 992px) {
  .content-wrapper {
    grid-template-columns: 1fr;
  }
  
  .summary-card {
    position: relative;
    top: 0;
    order: -1; /* Pone el resumen arriba en móviles para que se sepa qué se paga */
  }
}

@media (max-width: 600px) {
  .form-grid {
    grid-template-columns: 1fr;
  }
}
</style>
