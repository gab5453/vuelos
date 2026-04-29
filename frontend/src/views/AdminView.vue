<script setup>
import { ref, onMounted, watch } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'
import * as vuelosService from '../services/vuelos.service'
import * as crmService from '../services/crm.service'
import * as catalogosService from '../services/catalogos.service'
import * as auditoriaService from '../services/auditoria.service'

const router = useRouter()
const authStore = useAuthStore()

// Estado de navegación
const activeTab = ref('vuelos')
const isLoading = ref(false)
const isSubLoading = ref(false)
const error = ref(null)

// Datos
const vuelos = ref([])
const clientes = ref([])
const auditoria = ref([])
const aeropuertos = ref([])
const escalas = ref([])

// Filtros Auditoría
const auditFilters = ref({
  tabla: '',
  operacion: '',
  usuario: '',
  fecha_inicio: '',
  fecha_fin: '',
  page: 1,
  page_size: 20
})
const totalAuditPages = ref(1)

// Modales
const showVueloModal = ref(false)
const showLogModal = ref(false)
const showEscalasModal = ref(false)
const isEditing = ref(false)
const selectedLog = ref(null)
const selectedVuelo = ref(null)

const currentVuelo = ref({
  id_vuelo: null,
  numero_vuelo: '',
  id_aeropuerto_origen: null,
  id_aeropuerto_destino: null,
  fecha_salida: '',
  hora_salida: '',
  estado_vuelo: 'PROGRAMADO',
  precio_base: 0
})

const newEscala = ref({
  id_aeropuerto: null,
  orden: 1,
  duracion_minutos: 60,
  es_escala_tecnica: false
})

// Cargar datos según tab activa
const loadData = async () => {
  try {
    isLoading.value = true
    error.value = null

    if (activeTab.value === 'vuelos') {
      const response = await vuelosService.buscarVuelos({})
      vuelos.value = response.data || response
      if (aeropuertos.value.length === 0) {
        aeropuertos.value = await catalogosService.getAeropuertos()
      }
    } else if (activeTab.value === 'clientes') {
      const response = await crmService.getClientes({ page_size: 50 })
      clientes.value = response.data || response
    } else if (activeTab.value === 'auditoria') {
      const response = await auditoriaService.getAuditoria(auditFilters.value)
      auditoria.value = response.data || response
      totalAuditPages.value = response.total_pages || 1
    }
  } catch (err) {
    console.error('Error cargando datos de admin:', err)
    error.value = 'No se pudieron cargar los datos.'
  } finally {
    isLoading.value = false
  }
}

onMounted(() => {
  loadData()
})

watch(auditFilters, () => {
  if (activeTab.value === 'auditoria') loadData()
}, { deep: true })

// Acciones Vuelos
const openCreateVuelo = () => {
  isEditing.value = false
  currentVuelo.value = {
    numero_vuelo: '', id_aeropuerto_origen: null, id_aeropuerto_destino: null,
    fecha_salida: '', hora_salida: '', estado_vuelo: 'PROGRAMADO', precio_base: 100
  }
  showVueloModal.value = true
}

const handleSaveVuelo = async () => {
  try {
    if (isEditing.value) {
      await vuelosService.actualizarVuelo(currentVuelo.value.id_vuelo, currentVuelo.value)
    } else {
      await vuelosService.crearVuelo(currentVuelo.value)
    }
    showVueloModal.value = false
    loadData()
  } catch (err) { alert('Error al guardar vuelo') }
}

const toggleVueloStatus = async (vuelo) => {
  const nuevoEstado = vuelo.estado_vuelo === 'CANCELADO' ? 'PROGRAMADO' : 'CANCELADO'
  try {
    await vuelosService.cambiarEstadoVuelo(vuelo.id_vuelo || vuelo.guid_vuelo, nuevoEstado)
    loadData()
  } catch (err) { alert('Error al cambiar estado') }
}

// Acciones Escalas
const openManageEscalas = async (vuelo) => {
  selectedVuelo.value = vuelo
  showEscalasModal.value = true
  await loadEscalas()
}

const loadEscalas = async () => {
  try {
    isSubLoading.value = true
    const id = selectedVuelo.value.id_vuelo || selectedVuelo.value.guid_vuelo
    escalas.value = await vuelosService.getEscalas(id)
  } catch (err) {
    console.error('Error cargando escalas:', err)
    escalas.value = []
  } finally {
    isSubLoading.value = false
  }
}

const handleAddEscala = async () => {
  try {
    const id = selectedVuelo.value.id_vuelo || selectedVuelo.value.guid_vuelo
    await vuelosService.agregarEscala(id, newEscala.value)
    newEscala.value = { id_aeropuerto: null, orden: escalas.value.length + 1, duracion_minutos: 60, es_escala_tecnica: false }
    await loadEscalas()
  } catch (err) { alert('Error al agregar escala') }
}

const handleRemoveEscala = async (idEscala) => {
  if (!confirm('¿Seguro que desea eliminar esta escala?')) return
  try {
    const idVuelo = selectedVuelo.value.id_vuelo || selectedVuelo.value.guid_vuelo
    await vuelosService.eliminarEscala(idVuelo, idEscala)
    await loadEscalas()
  } catch (err) { alert('Error al eliminar escala') }
}

// Acciones Clientes
const toggleClienteStatus = async (cliente) => {
  const nuevoEstado = cliente.estado === 'INA' ? 'ACT' : 'INA'
  try {
    await crmService.cambiarEstadoCliente(cliente.guid_cliente, nuevoEstado)
    loadData()
  } catch (err) { alert('Error al cambiar estado') }
}

// Acciones Auditoría
const viewLogDetail = (log) => {
  selectedLog.value = log
  showLogModal.value = true
}

const resetAuditFilters = () => {
  auditFilters.value = { tabla: '', operacion: '', usuario: '', fecha_inicio: '', fecha_fin: '', page: 1, page_size: 20 }
}

// Helpers
const getAeropuertoName = (id) => {
  const aero = aeropuertos.value.find(a => a.id_aeropuerto === id)
  return aero ? aero.nombre : `ID: ${id}`
}

const formatDate = (dateStr) => {
  if (!dateStr) return '-'
  return new Date(dateStr).toLocaleString()
}
</script>

<template>
  <div class="admin-container">
    <aside class="admin-sidebar">
      <div class="sidebar-header"><div class="logo">✈️ AdminPanel</div></div>
      <nav class="sidebar-nav">
        <button :class="{ active: activeTab === 'vuelos' }" @click="activeTab = 'vuelos'; loadData()">📅 Gestión de Vuelos</button>
        <button :class="{ active: activeTab === 'clientes' }" @click="activeTab = 'clientes'; loadData()">👥 Usuarios y Clientes</button>
        <button :class="{ active: activeTab === 'auditoria' }" @click="activeTab = 'auditoria'; loadData()">📜 Auditoría Pro</button>
      </nav>
      <div class="sidebar-footer"><button @click="router.push('/')" class="btn-back">Volver al Sitio</button></div>
    </aside>

    <main class="admin-main">
      <header class="main-header">
        <div class="breadcrumb">Administración > <strong>{{ activeTab }}</strong></div>
        <div class="user-info"><span>Admin: <strong>{{ authStore.user?.nombre }}</strong></span></div>
      </header>

      <div class="content-card shadow-glass">
        <div v-if="isLoading" class="loading-overlay">
          <div class="spinner-box"><div class="spinner-inner"></div></div>
          <p>Sincronizando...</p>
        </div>

        <div v-if="error" class="error-msg">{{ error }}</div>

        <!-- VUELOS -->
        <section v-if="activeTab === 'vuelos' && !isLoading">
          <div class="section-header">
            <h3>Operaciones de Vuelo</h3>
            <button @click="openCreateVuelo" class="btn-add">+ Programar Vuelo</button>
          </div>
          <div class="table-container">
            <table class="admin-table">
              <thead>
                <tr>
                  <th>Vuelo</th>
                  <th>Ruta</th>
                  <th>Fecha/Hora</th>
                  <th>Escalas</th>
                  <th>Estado</th>
                  <th>Acciones</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="v in vuelos" :key="v.id_vuelo">
                  <td><span class="vuelo-code">{{ v.numero_vuelo }}</span></td>
                  <td>{{ getAeropuertoName(v.id_aeropuerto_origen) }} → {{ getAeropuertoName(v.id_aeropuerto_destino) }}</td>
                  <td>{{ formatDate(v.fecha_salida) }}</td>
                  <td>
                    <button @click="openManageEscalas(v)" class="btn-text">
                      ⚙️ Gestionar Escalas
                    </button>
                  </td>
                  <td><span :class="['badge', `badge-${v.estado_vuelo.toLowerCase()}`]">{{ v.estado_vuelo }}</span></td>
                  <td>
                    <button @click="toggleVueloStatus(v)" class="btn-icon">
                      {{ v.estado_vuelo === 'CANCELADO' ? '🔄' : '🚫' }}
                    </button>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
        </section>

        <!-- CLIENTES (Simplificado) -->
        <section v-else-if="activeTab === 'clientes' && !isLoading">
          <div class="section-header"><h3>Base de Clientes</h3></div>
          <div class="table-container">
            <table class="admin-table">
              <thead><tr><th>Nombre</th><th>ID</th><th>Email</th><th>Estado</th><th>Acciones</th></tr></thead>
              <tbody>
                <tr v-for="c in clientes" :key="c.id_cliente">
                  <td>{{ c.nombres }} {{ c.apellidos }}</td>
                  <td>{{ c.numero_identificacion }}</td>
                  <td>{{ c.correo }}</td>
                  <td><span :class="['badge', c.estado === 'ACT' ? 'badge-active' : 'badge-inactive']">{{ c.estado }}</span></td>
                  <td><button @click="toggleClienteStatus(c)" class="btn-toggle">{{ c.estado === 'ACT' ? 'Desactivar' : 'Activar' }}</button></td>
                </tr>
              </tbody>
            </table>
          </div>
        </section>

        <!-- AUDITORIA (Simplificado) -->
        <section v-else-if="activeTab === 'auditoria' && !isLoading">
          <div class="section-header"><h3>Historial de Auditoría</h3></div>
          <div class="filter-bar">
             <!-- Filtros aquí -->
          </div>
          <div class="table-container">
            <table class="admin-table table-audit">
              <thead><tr><th>Fecha</th><th>Usuario</th><th>Acción</th><th>Tabla</th><th>Descripción</th></tr></thead>
              <tbody>
                <tr v-for="log in auditoria" :key="log.id_auditoria" @click="viewLogDetail(log)" class="row-clickable">
                  <td>{{ formatDate(log.fecha_registro) }}</td>
                  <td>{{ log.usuario }}</td>
                  <td><span :class="['op-badge', `op-${log.operacion.toLowerCase()}`]">{{ log.operacion }}</span></td>
                  <td>{{ log.tabla }}</td>
                  <td class="truncate">{{ log.descripcion }}</td>
                </tr>
              </tbody>
            </table>
          </div>
        </section>
      </div>
    </main>

    <!-- Modal Escalas -->
    <div v-if="showEscalasModal" class="modal-overlay">
      <div class="modal-content escalas-modal">
        <div class="modal-header">
          <h2>Escalas del Vuelo: {{ selectedVuelo.numero_vuelo }}</h2>
          <button @click="showEscalasModal = false" class="btn-close">×</button>
        </div>
        
        <div class="escalas-body">
          <div v-if="isSubLoading" class="sub-loader">Cargando escalas...</div>
          
          <div v-else class="escalas-list">
            <div v-if="escalas.length === 0" class="empty-msg">Este vuelo es directo (sin escalas).</div>
            <div v-for="e in escalas" :key="e.id_escala" class="escala-item">
              <div class="escala-info">
                <span class="escala-order">#{{ e.orden }}</span>
                <strong>{{ getAeropuertoName(e.id_aeropuerto) }}</strong>
                <span class="escala-meta">{{ e.duracion_minutos }} min | {{ e.es_escala_tecnica ? 'Técnica' : 'Comercial' }}</span>
              </div>
              <button @click="handleRemoveEscala(e.id_escala)" class="btn-delete-escala">🗑️</button>
            </div>
          </div>

          <hr class="modal-divider">

          <h3>Agregar Nueva Escala</h3>
          <form @submit.prevent="handleAddEscala" class="escala-form">
            <div class="input-group">
              <label>Aeropuerto de Parada</label>
              <select v-model="newEscala.id_aeropuerto" required>
                <option v-for="a in aeropuertos" :key="a.id_aeropuerto" :value="a.id_aeropuerto">
                  {{ a.nombre }} ({{ a.iata }})
                </option>
              </select>
            </div>
            <div class="grid-3">
              <div class="input-group">
                <label>Orden</label>
                <input type="number" v-model="newEscala.orden" required min="1" />
              </div>
              <div class="input-group">
                <label>Minutos</label>
                <input type="number" v-model="newEscala.duracion_minutos" required min="1" />
              </div>
              <div class="input-group check-group">
                <label><input type="checkbox" v-model="newEscala.es_escala_tecnica" /> Técnica</label>
              </div>
            </div>
            <button type="submit" class="btn-submit">Añadir Parada</button>
          </form>
        </div>
      </div>
    </div>

    <!-- Otros Modales (Vuelo, Log) -->
    <!-- ... (Mantenemos los de antes) -->
    <div v-if="showLogModal" class="modal-overlay" @click.self="showLogModal = false">
       <div class="modal-content"> ... </div>
    </div>
  </div>
</template>

<style scoped>
/* Copiamos estilos anteriores y agregamos específicos para escalas */
@import url('https://fonts.googleapis.com/css2?family=Inter:wght@400;600;700;800&display=swap');

.escalas-modal {
  max-width: 500px;
}

.modal-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 24px;
}

.btn-close {
  background: none;
  border: none;
  font-size: 2rem;
  cursor: pointer;
  color: #94a3b8;
}

.escalas-list {
  display: flex;
  flex-direction: column;
  gap: 12px;
  margin-bottom: 24px;
  max-height: 250px;
  overflow-y: auto;
  padding-right: 10px;
}

.escala-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 12px;
  background: #f8fafc;
  border-radius: 10px;
  border: 1px solid #e2e8f0;
}

.escala-order {
  background: #3b82f6;
  color: white;
  padding: 2px 8px;
  border-radius: 4px;
  font-size: 0.8rem;
  font-weight: 800;
  margin-right: 12px;
}

.escala-meta {
  display: block;
  font-size: 0.8rem;
  color: #64748b;
}

.btn-delete-escala {
  background: none;
  border: none;
  cursor: pointer;
  font-size: 1.2rem;
}

.modal-divider {
  margin: 24px 0;
  border: none;
  border-top: 1px solid #e2e8f0;
}

.escala-form {
  display: flex;
  flex-direction: column;
  gap: 16px;
}

.grid-3 {
  display: grid;
  grid-template-columns: 1fr 1fr 1fr;
  gap: 12px;
}

.check-group {
  justify-content: center;
  align-items: center;
}

.empty-msg {
  text-align: center;
  color: #94a3b8;
  padding: 20px;
  font-style: italic;
}

/* Reutilizando estilos de AdminView... */
.admin-container { display: flex; min-height: 100vh; background-color: #f8fafc; }
.admin-sidebar { width: 280px; background: #0f172a; color: white; display: flex; flex-direction: column; }
.admin-main { flex: 1; padding: 40px; }
.shadow-glass { background: white; border-radius: 24px; padding: 32px; box-shadow: 0 10px 40px rgba(0,0,0,0.03); }
.admin-table { width: 100%; border-collapse: separate; border-spacing: 0 8px; }
.admin-table td { padding: 16px; background: #fff; border-top: 1px solid #f1f5f9; border-bottom: 1px solid #f1f5f9; }
.modal-overlay { position: fixed; top: 0; left: 0; width: 100%; height: 100%; background: rgba(15, 23, 42, 0.6); backdrop-filter: blur(4px); display: flex; align-items: center; justify-content: center; z-index: 1000; }
.modal-content { background: white; padding: 40px; border-radius: 24px; width: 90%; }
.btn-submit { background: #2563eb; color: white; padding: 12px; border-radius: 8px; font-weight: 700; width: 100%; }
.input-group { display: flex; flex-direction: column; gap: 6px; }
.input-group label { font-weight: 600; font-size: 0.9rem; }
.input-group select, .input-group input { padding: 10px; border-radius: 8px; border: 1px solid #e2e8f0; }
.badge { padding: 6px 12px; border-radius: 20px; font-size: 0.75rem; font-weight: 800; }
.badge-programado { background: #dcfce7; color: #166534; }
.badge-cancelado { background: #fee2e2; color: #991b1b; }
</style>
