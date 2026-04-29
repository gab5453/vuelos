<script setup>
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'
import { getReservas, cambiarEstadoReserva, getBoletos, getFacturas } from '../services/ventas.service'

const router = useRouter()
const authStore = useAuthStore()

const reservations = ref([])
const isLoading = ref(true)
const error = ref(null)

onMounted(async () => {
  if (!authStore.isAuthenticated || !authStore.user) {
    router.push('/')
    return
  }

  try {
    // Intentamos consumir la API real pasando el ID del cliente
    // Asumimos que authStore.user.id es el equivalente a id_cliente
    const data = await getReservas({ id_cliente: authStore.user.id })
    reservations.value = data || []
  } catch (err) {
    console.warn('No se pudo conectar a la API de Reservas. Cargando datos de prueba.')
    error.value = 'Mostrando datos simulados.'
    loadMockReservations()
  } finally {
    isLoading.value = false
  }
})

const cancelReservation = async (idReserva) => {
  if (!confirm('¿Estás seguro de que deseas cancelar esta reserva?')) return

  try {
    await cambiarEstadoReserva(idReserva, 'CAN')
    // Actualizar lista local
    const res = reservations.value.find(r => r.id_reserva === idReserva)
    if (res) res.estado_reserva = 'CAN'
    alert('Reserva cancelada exitosamente.')
  } catch (err) {
    alert('No se pudo cancelar la reserva. Por favor intenta más tarde.')
  }
}

const showTicketModal = ref(false)
const showInvoiceModal = ref(false)
const selectedTicket = ref(null)
const selectedInvoice = ref(null)

const viewTicket = async (idReserva) => {
  try {
    const data = await getBoletos({ id_reserva: idReserva })
    if (data && data.length > 0) {
      selectedTicket.value = data[0]
      showTicketModal.value = true
    } else {
      alert('Boleto no encontrado para esta reserva.')
    }
  } catch (err) {
    alert('Error al obtener el boleto.')
  }
}

const viewInvoice = async (idReserva) => {
  try {
    const data = await getFacturas({ id_reserva: idReserva })
    if (data && data.length > 0) {
      selectedInvoice.value = data[0]
      showInvoiceModal.value = true
    } else {
      alert('Factura no encontrada para esta reserva.')
    }
  } catch (err) {
    alert('Error al obtener la factura.')
  }
}

const loadMockReservations = () => {
  reservations.value = [
    {
      id_reserva: 1001,
      fecha_inicio: new Date().toISOString(),
      estado_reserva: 'EMI',
      total_reserva: 895.00,
      vuelo: {
        origen: 'MAD',
        destino: 'MEX',
        fecha_salida: '15 Oct 2026 10:30'
      },
      asiento: '1A'
    },
    {
      id_reserva: 1002,
      fecha_inicio: new Date(Date.now() - 86400000 * 5).toISOString(),
      estado_reserva: 'CAN',
      total_reserva: 620.00,
      vuelo: {
        origen: 'JFK',
        destino: 'LHR',
        fecha_salida: '01 Nov 2026 18:00'
      },
      asiento: '12C'
    }
  ]
}

const getStatusClass = (estado) => {
  if (estado === 'EMI') return 'status-emitida'
  if (estado === 'PEN') return 'status-pendiente'
  if (estado === 'CAN') return 'status-cancelada'
  return 'status-default'
}

const getStatusLabel = (estado) => {
  if (estado === 'EMI') return 'Emitida'
  if (estado === 'PEN') return 'Pendiente'
  if (estado === 'CAN') return 'Cancelada'
  return estado
}

const formatDate = (dateString) => {
  const options = { year: 'numeric', month: 'long', day: 'numeric' }
  return new Date(dateString).toLocaleDateString('es-ES', options)
}
</script>

<template>
  <div class="reservations-container">
    <div class="header">
      <div class="container">
        <h1>Mis Viajes y Reservas</h1>
        <p>Administra todos tus boletos en un solo lugar</p>
      </div>
    </div>

    <div class="content-wrapper container">
      <div v-if="isLoading" class="loading-state">
        <div class="spinner">⏳</div>
        <p>Buscando tus reservas...</p>
      </div>

      <div v-else-if="reservations.length === 0" class="empty-state">
        <div class="empty-icon">🎒</div>
        <h2>Aún no tienes viajes programados</h2>
        <p>Explora el mundo con nuestras ofertas y tu próxima aventura aparecerá aquí.</p>
        <button @click="router.push('/')" class="btn-primary">Buscar Vuelos</button>
      </div>

      <div v-else class="reservations-list">
        <div v-if="error" class="error-banner">
          ⚠️ {{ error }}
        </div>
        
        <div v-for="reserva in reservations" :key="reserva.id_reserva" class="reservation-card">
          <div class="card-header">
            <span class="reserva-id">Reserva #{{ reserva.id_reserva }}</span>
            <span class="status-badge" :class="getStatusClass(reserva.estado_reserva)">
              {{ getStatusLabel(reserva.estado_reserva) }}
            </span>
          </div>
          
          <div class="card-body">
            <div class="flight-info" v-if="reserva.vuelo">
              <div class="route">
                <strong>{{ reserva.vuelo.origen }}</strong>
                <span class="arrow">✈️</span>
                <strong>{{ reserva.vuelo.destino }}</strong>
              </div>
              <div class="details">
                <p><strong>Salida:</strong> {{ reserva.vuelo.fecha_salida }}</p>
                <p><strong>Asiento:</strong> {{ reserva.asiento || 'Pendiente' }}</p>
              </div>
            </div>
            <div class="flight-info" v-else>
              <p>Datos del vuelo no disponibles en la pre-reserva.</p>
            </div>
            
            <div class="price-info">
              <p>Total Pagado</p>
              <h3>${{ reserva.total_reserva }}</h3>
              <p class="date-booked">Comprado el: {{ formatDate(reserva.fecha_inicio) }}</p>
            </div>
          </div>
          
          <div class="card-footer">
            <button class="btn-secondary" v-if="reserva.estado_reserva === 'EMI'" @click="viewTicket(reserva.id_reserva)">Ver Boleto</button>
            <button class="btn-secondary" v-if="reserva.estado_reserva === 'EMI'" @click="viewInvoice(reserva.id_reserva)">Ver Factura</button>
            <button class="btn-danger" v-if="reserva.estado_reserva === 'PEN'" @click="cancelReservation(reserva.id_reserva)">Cancelar Reserva</button>
          </div>
        </div>
      </div>
    </div>

    <!-- Modal de Boleto -->
    <div v-if="showTicketModal" class="modal-overlay" @click.self="showTicketModal = false">
      <div class="modal-content ticket-modal">
        <div class="ticket-header">
          <div class="airline-tag">VuelosPro</div>
          <div class="ticket-status">BOARDING PASS</div>
        </div>
        <div class="ticket-body" v-if="selectedTicket">
          <div class="ticket-row">
            <div class="item">
              <label>PASAJERO</label>
              <span>{{ authStore.user.nombre }}</span>
            </div>
            <div class="item">
              <label>VUELO</label>
              <span>{{ selectedTicket.id_vuelo }}</span>
            </div>
          </div>
          <div class="ticket-row">
            <div class="item">
              <label>ASIENTO</label>
              <span class="highlight">{{ selectedTicket.clase }}</span>
            </div>
            <div class="item">
              <label>CODIGO</label>
              <span>{{ selectedTicket.codigo_boleto || 'BT-882' }}</span>
            </div>
          </div>
        </div>
        <button @click="showTicketModal = false" class="btn-close-modal">Cerrar</button>
      </div>
    </div>

    <!-- Modal de Factura -->
    <div v-if="showInvoiceModal" class="modal-overlay" @click.self="showInvoiceModal = false">
      <div class="modal-content invoice-modal">
        <div class="invoice-header">
          <h2>DETALLE DE FACTURA</h2>
          <span class="invoice-number">{{ selectedInvoice?.numero_factura }}</span>
        </div>
        <div class="invoice-body" v-if="selectedInvoice">
          <div class="invoice-row header-row">
            <span>Descripción</span>
            <span>Total</span>
          </div>
          <hr>
          <div class="invoice-row">
            <span>Boleto Aéreo + Tasas</span>
            <span>${{ selectedInvoice.subtotal }}</span>
          </div>
          <div class="invoice-row">
            <span>IVA</span>
            <span>${{ selectedInvoice.valor_iva }}</span>
          </div>
          <div class="invoice-row total-row">
            <span>TOTAL PAGADO</span>
            <span>${{ selectedInvoice.total }}</span>
          </div>
          <div class="invoice-footer">
            <p>Fecha: {{ formatDate(selectedInvoice.fecha_emision) }}</p>
            <p>Gracias por su compra</p>
          </div>
        </div>
        <button @click="showInvoiceModal = false" class="btn-close-modal">Cerrar</button>
      </div>
    </div>
  </div>
</template>

<style scoped>
.reservations-container {
  min-height: 100vh;
  background-color: #f8fafc;
  padding-bottom: 60px;
}

.header {
  background: var(--primary);
  color: white;
  padding: 40px 20px;
  margin-bottom: 40px;
  box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1);
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
  max-width: 900px;
  margin: 0 auto;
}

.loading-state, .empty-state {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: 80px 20px;
  text-align: center;
  background: white;
  border-radius: 12px;
  box-shadow: 0 4px 6px -1px rgba(0,0,0,0.05);
}

.spinner, .empty-icon {
  font-size: 4rem;
  margin-bottom: 24px;
}

.empty-state h2 {
  color: #1e293b;
  margin-bottom: 12px;
}

.empty-state p {
  color: #64748b;
  margin-bottom: 24px;
}

.btn-primary {
  background: var(--primary);
  color: white;
  padding: 12px 24px;
  border-radius: 8px;
  font-weight: 600;
  border: none;
  cursor: pointer;
}

.btn-primary:hover {
  background: #1d4ed8;
}

.error-banner {
  background-color: #fffbeb;
  color: #b45309;
  padding: 16px;
  border-radius: 8px;
  border: 1px solid #fef3c7;
  margin-bottom: 24px;
  font-weight: 500;
}

.reservations-list {
  display: flex;
  flex-direction: column;
  gap: 24px;
  padding: 0 20px;
}

.reservation-card {
  background: white;
  border-radius: 12px;
  box-shadow: 0 4px 15px rgba(0,0,0,0.05);
  overflow: hidden;
  border: 1px solid #e2e8f0;
}

.card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 16px 24px;
  background: #f1f5f9;
  border-bottom: 1px solid #e2e8f0;
}

.reserva-id {
  font-weight: 600;
  color: #475569;
}

.status-badge {
  padding: 6px 12px;
  border-radius: 20px;
  font-size: 0.85rem;
  font-weight: 700;
  text-transform: uppercase;
}

.status-emitida { background: #dcfce7; color: #166534; }
.status-pendiente { background: #fef9c3; color: #854d0e; }
.status-cancelada { background: #fee2e2; color: #991b1b; }
.status-default { background: #e2e8f0; color: #475569; }

.card-body {
  display: flex;
  justify-content: space-between;
  padding: 24px;
}

.route {
  display: flex;
  align-items: center;
  gap: 16px;
  font-size: 1.5rem;
  color: #1e293b;
  margin-bottom: 12px;
}

.arrow {
  color: #94a3b8;
  font-size: 1.2rem;
}

.details p {
  color: #475569;
  margin-bottom: 8px;
}

.price-info {
  text-align: right;
  display: flex;
  flex-direction: column;
  justify-content: center;
}

.price-info p {
  color: #64748b;
  font-size: 0.9rem;
  margin: 0;
}

.price-info h3 {
  font-size: 1.8rem;
  color: #1e293b;
  margin: 4px 0 8px 0;
}

.date-booked {
  font-size: 0.8rem !important;
}

.card-footer {
  padding: 16px 24px;
  background: #f8fafc;
  border-top: 1px solid #e2e8f0;
  display: flex;
  gap: 12px;
  justify-content: flex-end;
}

.btn-secondary {
  background: white;
  border: 1px solid #cbd5e1;
  color: #475569;
  padding: 8px 16px;
  border-radius: 6px;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.2s;
}

.btn-secondary:hover {
  background: #f1f5f9;
  border-color: #94a3b8;
}

.btn-danger {
  background: white;
  border: 1px solid #fecaca;
  color: #ef4444;
  padding: 8px 16px;
  border-radius: 6px;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.2s;
}

.btn-danger:hover {
  background: #fef2f2;
  border-color: #fca5a5;
}

@media (max-width: 600px) {
  .card-body {
    flex-direction: column;
    gap: 20px;
  }
  .price-info {
    text-align: left;
    border-top: 1px dashed #e2e8f0;
    padding-top: 16px;
  }
  .card-footer {
    justify-content: stretch;
    flex-direction: column;
  }
  .card-footer button {
    width: 100%;
  }
}

/* Modales */
.modal-overlay {
  position: fixed;
  top: 0; left: 0; width: 100%; height: 100%;
  background: rgba(0,0,0,0.6);
  backdrop-filter: blur(4px);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
  padding: 20px;
}

.modal-content {
  background: white;
  border-radius: 20px;
  width: 100%;
  max-width: 450px;
  padding: 30px;
  box-shadow: 0 20px 40px rgba(0,0,0,0.3);
  animation: slideUp 0.3s ease-out;
}

@keyframes slideUp {
  from { transform: translateY(30px); opacity: 0; }
  to { transform: translateY(0); opacity: 1; }
}

.btn-close-modal {
  width: 100%;
  margin-top: 24px;
  padding: 12px;
  border-radius: 8px;
  border: 1px solid #e2e8f0;
  background: #f8fafc;
  font-weight: 600;
  cursor: pointer;
}

/* Ticket Style */
.ticket-modal {
  background: #fff;
  border: 2px solid #e2e8f0;
  padding: 0;
  overflow: hidden;
}

.ticket-header {
  background: #2563eb;
  color: white;
  padding: 20px;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.airline-tag { font-weight: 800; font-size: 1.2rem; }
.ticket-status { font-size: 0.8rem; font-weight: 600; opacity: 0.8; }

.ticket-body { padding: 30px; }
.ticket-row { display: flex; gap: 40px; margin-bottom: 24px; }
.ticket-row .item { display: flex; flex-direction: column; gap: 4px; }
.ticket-row .item label { font-size: 0.7rem; color: #94a3b8; font-weight: 700; }
.ticket-row .item span { font-weight: 700; color: #1e293b; font-size: 1.1rem; }
.highlight { color: #2563eb !important; }

.qr-placeholder {
  margin-top: 32px;
  text-align: center;
  border-top: 2px dashed #e2e8f0;
  padding-top: 24px;
}

.qr-box {
  background: #f1f5f9;
  height: 60px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-family: monospace;
  letter-spacing: 10px;
  font-size: 1.5rem;
  margin-bottom: 8px;
}

/* Invoice Style */
.invoice-modal { padding: 40px; }
.invoice-header { margin-bottom: 32px; border-bottom: 2px solid #f1f5f9; padding-bottom: 16px; }
.invoice-number { color: #64748b; font-size: 0.9rem; }
.invoice-row { display: flex; justify-content: space-between; margin-bottom: 12px; }
.header-row { font-weight: 700; color: #94a3b8; font-size: 0.85rem; }
.total-row { margin-top: 24px; padding-top: 16px; border-top: 2px solid #f1f5f9; font-weight: 800; font-size: 1.2rem; color: #1e293b; }
.invoice-footer { margin-top: 32px; text-align: center; color: #94a3b8; font-size: 0.85rem; }
</style>
