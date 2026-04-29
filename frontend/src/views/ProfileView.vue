<script setup>
import { ref, onMounted, watch, computed } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'
import * as crmService from '../services/crm.service'
import * as catalogosService from '../services/catalogos.service'

const router = useRouter()
const authStore = useAuthStore()

// Estado
const isLoading = ref(true)
const isSaving = ref(false)
const error = ref(null)
const successMsg = ref(null)

const paises = ref([])
const ciudades = ref([])
const profileData = ref({
  guid_cliente: '',
  nombres: '',
  apellidos: '',
  razon_social: '',
  tipo_identificacion: '',
  numero_identificacion: '',
  correo: '',
  telefono: '',
  direccion: '',
  id_ciudad_residencia: null,
  id_pais_nacionalidad: null,
  fecha_nacimiento: '',
  nacionalidad: '',
  genero: '',
  estado: 'ACT'
})

// Cargar datos iniciales
onMounted(async () => {
  if (!authStore.isAuthenticated) {
    router.push('/auth')
    return
  }

  try {
    isLoading.value = true
    
    // 1. Cargar Catálogos
    paises.value = await catalogosService.getPaises()

    // 2. Cargar Datos del Cliente
    // Intentamos por identificación si no tenemos el GUID en el store
    const user = authStore.user
    let cliente = null

    if (user.guid_cliente) {
      cliente = await crmService.getClienteByGuid(user.guid_cliente)
    } else if (user.identificacion || user.numero_identificacion) {
      cliente = await crmService.getClienteByIdentificacion(
        user.tipo_id || user.tipo_identificacion, 
        user.identificacion || user.numero_identificacion
      )
    }

    if (cliente) {
      // Mapear datos recibidos al formulario
      profileData.value = {
        ...profileData.value,
        ...cliente,
        // Asegurar formato de fecha para el input type="date"
        fecha_nacimiento: cliente.fecha_nacimiento ? cliente.fecha_nacimiento.split('T')[0] : ''
      }
      
      // Si ya tiene país, cargar sus ciudades
      if (profileData.value.id_pais_nacionalidad) {
        ciudades.value = await catalogosService.getCiudades({ id_pais: profileData.value.id_pais_nacionalidad })
      }
    }
  } catch (err) {
    console.error('Error cargando perfil:', err)
    error.value = 'No se pudo cargar la información del perfil.'
  } finally {
    isLoading.value = false
  }
})

// Watcher para cambios de país
watch(() => profileData.value.id_pais_nacionalidad, async (newPaisId) => {
  if (newPaisId) {
    try {
      ciudades.value = await catalogosService.getCiudades({ id_pais: newPaisId })
    } catch (err) {
      console.error('Error cargando ciudades:', err)
    }
  } else {
    ciudades.value = []
  }
})

const handleSave = async () => {
  try {
    isSaving.value = true
    error.value = null
    successMsg.value = null

    const guid = profileData.value.guid_cliente
    if (!guid) throw new Error('No se encontró el identificador del cliente.')

    const result = await crmService.actualizarCliente(guid, profileData.value)
    
    // Actualizar store global
    authStore.updateUserData({
      nombre: `${profileData.value.nombres} ${profileData.value.apellidos}`,
      guid_cliente: guid
    })

    successMsg.value = 'Perfil actualizado con éxito.'
    
    // Ocultar mensaje de éxito después de unos segundos
    setTimeout(() => { successMsg.value = null }, 5000)
  } catch (err) {
    console.error('Error guardando perfil:', err)
    error.value = 'Ocurrió un error al intentar guardar los cambios.'
  } finally {
    isSaving.value = false
  }
}
</script>

<template>
  <div class="profile-container">
    <div class="header-section">
      <div class="container">
        <h1>Mi Perfil</h1>
        <p>Gestiona tu información personal y de residencia</p>
      </div>
    </div>

    <div class="container main-content">
      <div v-if="isLoading" class="loading-state">
        <div class="spinner">✈️</div>
        <p>Cargando información...</p>
      </div>

      <div v-else class="profile-layout">
        <!-- Sidebar de Resumen -->
        <div class="profile-sidebar">
          <div class="user-card glass-panel">
            <div class="avatar-box">
              {{ profileData.nombres?.charAt(0) || 'U' }}
            </div>
            <h3>{{ profileData.nombres }} {{ profileData.apellidos }}</h3>
            <span class="role-badge">{{ authStore.user?.rol || 'Cliente' }}</span>
            <div class="stats">
              <div class="stat-item">
                <span class="label">Miembro desde</span>
                <span class="value">2024</span>
              </div>
            </div>
          </div>
        </div>

        <!-- Formulario Principal -->
        <div class="profile-form-container">
          <form @submit.prevent="handleSave" class="glass-panel profile-form">
            
            <div v-if="error" class="alert alert-error">{{ error }}</div>
            <div v-if="successMsg" class="alert alert-success">{{ successMsg }}</div>

            <div class="form-section">
              <h2 class="section-title">Datos Personales</h2>
              <div class="grid-2">
                <div class="input-group">
                  <label>Nombres</label>
                  <input type="text" v-model="profileData.nombres" required />
                </div>
                <div class="input-group">
                  <label>Apellidos</label>
                  <input type="text" v-model="profileData.apellidos" required />
                </div>
              </div>

              <div class="grid-2">
                <div class="input-group">
                  <label>Tipo Documento</label>
                  <select v-model="profileData.tipo_identificacion" disabled>
                    <option value="CEDULA">Cédula</option>
                    <option value="PASAPORTE">Pasaporte</option>
                    <option value="RUC">RUC</option>
                  </select>
                </div>
                <div class="input-group">
                  <label>Nº Documento</label>
                  <input type="text" v-model="profileData.numero_identificacion" readonly />
                </div>
              </div>

              <div class="grid-2">
                <div class="input-group">
                  <label>Fecha de Nacimiento</label>
                  <input 
                    type="date" 
                    v-model="profileData.fecha_nacimiento" 
                    @click="$event.target.showPicker()"
                  />
                </div>
                <div class="input-group">
                  <label>Género</label>
                  <select v-model="profileData.genero">
                    <option value="">Seleccione...</option>
                    <option value="M">Masculino</option>
                    <option value="F">Femenino</option>
                    <option value="O">Otro</option>
                  </select>
                </div>
              </div>

              <div class="input-group">
                <label>Nacionalidad (Texto)</label>
                <input type="text" v-model="profileData.nacionalidad" placeholder="Ej: Ecuatoriana" />
              </div>
            </div>

            <div class="form-section">
              <h2 class="section-title">Contacto y Residencia</h2>
              <div class="input-group">
                <label>Correo Electrónico</label>
                <input type="email" v-model="profileData.correo" required />
              </div>
              
              <div class="grid-2">
                <div class="input-group">
                  <label>Teléfono</label>
                  <input type="tel" v-model="profileData.telefono" placeholder="+593..." />
                </div>
                <div class="input-group">
                  <label>País de Residencia</label>
                  <select v-model="profileData.id_pais_nacionalidad">
                    <option :value="null">Seleccione un país...</option>
                    <option v-for="p in paises" :key="p.id_pais" :value="p.id_pais">
                      {{ p.nombre }}
                    </option>
                  </select>
                </div>
              </div>

              <div class="grid-2">
                <div class="input-group">
                  <label>Ciudad</label>
                  <select v-model="profileData.id_ciudad_residencia" :disabled="!profileData.id_pais_nacionalidad">
                    <option :value="null">Seleccione una ciudad...</option>
                    <option v-for="c in ciudades" :key="c.id_ciudad" :value="c.id_ciudad">
                      {{ c.nombre }}
                    </option>
                  </select>
                </div>
                <div class="input-group">
                  <label>Dirección</label>
                  <input type="text" v-model="profileData.direccion" placeholder="Calle principal y secundaria" />
                </div>
              </div>
            </div>

            <div class="form-actions">
              <button type="submit" class="btn-save" :disabled="isSaving">
                {{ isSaving ? 'Guardando...' : 'Guardar Cambios' }}
              </button>
            </div>

          </form>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.profile-container {
  min-height: 100vh;
  background-color: #f0f2f5;
  padding-bottom: 80px;
}

.header-section {
  background: linear-gradient(135deg, #1e293b 0%, #334155 100%);
  color: white;
  padding: 60px 0;
  margin-bottom: -40px;
}

.header-section h1 {
  font-size: 2.5rem;
  margin-bottom: 8px;
}

.header-section p {
  opacity: 0.8;
  font-size: 1.1rem;
}

.container {
  max-width: 1100px;
  margin: 0 auto;
  padding: 0 20px;
}

.main-content {
  position: relative;
  z-index: 10;
}

.profile-layout {
  display: grid;
  grid-template-columns: 300px 1fr;
  gap: 32px;
}

.glass-panel {
  background: rgba(255, 255, 255, 0.95);
  backdrop-filter: blur(10px);
  border-radius: 20px;
  border: 1px solid rgba(255, 255, 255, 0.3);
  box-shadow: 0 10px 25px rgba(0,0,0,0.05);
  padding: 32px;
}

/* User Card */
.user-card {
  text-align: center;
}

.avatar-box {
  width: 100px;
  height: 100px;
  background: linear-gradient(135deg, var(--primary), #818cf8);
  color: white;
  font-size: 3rem;
  font-weight: 700;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  margin: 0 auto 20px;
  box-shadow: 0 8px 16px rgba(99, 102, 241, 0.3);
}

.user-card h3 {
  font-size: 1.4rem;
  margin-bottom: 8px;
  color: #1e293b;
}

.role-badge {
  display: inline-block;
  background: #e0e7ff;
  color: #4338ca;
  padding: 4px 12px;
  border-radius: 20px;
  font-size: 0.8rem;
  font-weight: 700;
  text-transform: uppercase;
  margin-bottom: 24px;
}

.stats {
  border-top: 1px solid #e2e8f0;
  padding-top: 20px;
}

.stat-item {
  display: flex;
  flex-direction: column;
  gap: 4px;
}

.stat-item .label {
  font-size: 0.75rem;
  color: #64748b;
  text-transform: uppercase;
}

.stat-item .value {
  font-weight: 600;
  color: #1e293b;
}

/* Form Styles */
.section-title {
  font-size: 1.3rem;
  color: #1e293b;
  margin-bottom: 24px;
  padding-bottom: 12px;
  border-bottom: 2px solid #f1f5f9;
}

.form-section {
  margin-bottom: 40px;
}

.grid-2 {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 20px;
}

.input-group {
  margin-bottom: 20px;
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.input-group label {
  font-size: 0.9rem;
  font-weight: 600;
  color: #475569;
}

.input-group input,
.input-group select {
  padding: 12px 16px;
  border-radius: 10px;
  border: 2px solid #e2e8f0;
  font-size: 1rem;
  transition: all 0.3s;
}

.input-group input:focus,
.input-group select:focus {
  border-color: var(--primary);
  box-shadow: 0 0 0 4px rgba(99, 102, 241, 0.1);
  outline: none;
}

.input-group input[readonly],
.input-group select[disabled] {
  background: #f8fafc;
  cursor: not-allowed;
}

.btn-save {
  background: var(--primary);
  color: white;
  padding: 16px 32px;
  border-radius: 12px;
  font-weight: 700;
  font-size: 1.1rem;
  border: none;
  cursor: pointer;
  box-shadow: 0 10px 15px -3px rgba(99, 102, 241, 0.4);
  transition: all 0.3s;
}

.btn-save:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 20px 25px -5px rgba(99, 102, 241, 0.3);
  background: var(--primary-hover);
}

.btn-save:disabled {
  opacity: 0.7;
  cursor: not-allowed;
}

/* Alerts */
.alert {
  padding: 16px;
  border-radius: 12px;
  margin-bottom: 24px;
  font-weight: 500;
}

.alert-error {
  background: #fef2f2;
  color: #ef4444;
  border: 1px solid #fecaca;
}

.alert-success {
  background: #f0fdf4;
  color: #15803d;
  border: 1px solid #bbf7d0;
}

/* Loading */
.loading-state {
  text-align: center;
  padding: 100px 0;
}

.spinner {
  font-size: 3rem;
  animation: pulse 1.5s infinite;
}

@keyframes pulse {
  0% { transform: scale(1); opacity: 1; }
  50% { transform: scale(1.2); opacity: 0.7; }
  100% { transform: scale(1); opacity: 1; }
}

@media (max-width: 850px) {
  .profile-layout {
    grid-template-columns: 1fr;
  }
  .grid-2 {
    grid-template-columns: 1fr;
  }
}
</style>
