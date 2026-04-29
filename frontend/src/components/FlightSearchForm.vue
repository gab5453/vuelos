<script setup>
import { ref, computed, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useFlightStore } from '../stores/index'
import { getAeropuertos } from '../services/catalogos.service'

const router = useRouter()
const flightStore = useFlightStore()

const today = computed(() => new Date().toISOString().split('T')[0])
const minReturnDate = computed(() => searchData.value.departureDate || today.value)

// Lista de aeropuertos
const airports = ref([])
const isLoadingAirports = ref(true)

onMounted(async () => {
  try {
    isLoadingAirports.value = true
    // Intenta traer los aeropuertos reales del microservicio
    const data = await getAeropuertos()
    airports.value = data
  } catch (error) {
    console.error('Error al cargar aeropuertos:', error)
    airports.value = []
  } finally {
    isLoadingAirports.value = false
  }
})

const searchData = ref({
  origin: '',
  destination: '',
  departureDate: '',
  returnDate: '',
  passengers: 1,
  type: 'roundtrip' // 'roundtrip' o 'oneway'
})

const submitSearch = async () => {
  if (searchData.value.origin === searchData.value.destination) {
    alert('El origen y el destino no pueden ser iguales.')
    return
  }
  
  // Ejecutamos la búsqueda en Pinia (llama al API)
  await flightStore.searchFlights(searchData.value)
}
</script>

<template>
  <div class="glass-form-container">
    <div class="trip-types">
      <label class="radio-label">
        <input type="radio" v-model="searchData.type" value="roundtrip">
        Ida y Vuelta
      </label>
      <label class="radio-label">
        <input type="radio" v-model="searchData.type" value="oneway">
        Solo Ida
      </label>
    </div>

    <form @submit.prevent="submitSearch" class="search-form">
      <div class="input-group">
        <label>Origen</label>
        <div class="input-wrapper">
          <span class="icon">🛫</span>
          <select v-model="searchData.origin" required>
            <option value="" disabled selected>{{ isLoadingAirports ? 'Cargando aeropuertos...' : '¿De dónde sales?' }}</option>
            <option v-for="airport in airports" :key="airport.id_aeropuerto" :value="airport.id_aeropuerto" :disabled="searchData.destination === airport.id_aeropuerto">
              {{ airport.nombre }} ({{ airport.codigo_iata }})
            </option>
          </select>
        </div>
      </div>

      <div class="input-group">
        <label>Destino</label>
        <div class="input-wrapper">
          <span class="icon">🛬</span>
          <select v-model="searchData.destination" required>
            <option value="" disabled selected>{{ isLoadingAirports ? 'Cargando aeropuertos...' : '¿A dónde vas?' }}</option>
            <option v-for="airport in airports" :key="airport.id_aeropuerto" :value="airport.id_aeropuerto" :disabled="searchData.origin === airport.id_aeropuerto">
              {{ airport.nombre }} ({{ airport.codigo_iata }})
            </option>
          </select>
        </div>
      </div>

      <div class="input-group">
        <label>Ida</label>
        <div class="input-wrapper">
          <input type="date" v-model="searchData.departureDate" :min="today" required @click="$event.target.showPicker()">
        </div>
      </div>

      <div class="input-group" v-if="searchData.type === 'roundtrip'">
        <label>Vuelta</label>
        <div class="input-wrapper">
          <input type="date" v-model="searchData.returnDate" :min="minReturnDate" required @click="$event.target.showPicker()">
        </div>
      </div>

      <div class="input-group passengers">
        <label>Pasajeros</label>
        <div class="input-wrapper">
          <span class="icon">👥</span>
          <input type="number" v-model="searchData.passengers" min="1" max="9" required>
        </div>
      </div>

      <button type="submit" class="search-btn">
        Buscar Vuelos
      </button>
    </form>
  </div>
</template>

<style scoped>
.glass-form-container {
  background: var(--card-bg);
  backdrop-filter: blur(16px);
  -webkit-backdrop-filter: blur(16px);
  border: 1px solid var(--border-color);
  border-radius: var(--radius-lg);
  padding: 32px;
  box-shadow: var(--shadow-glass);
  width: 100%;
  max-width: 1000px;
  margin: 0 auto;
}

.trip-types {
  display: flex;
  gap: 24px;
  margin-bottom: 24px;
  font-weight: 500;
}

.radio-label {
  display: flex;
  align-items: center;
  gap: 8px;
  cursor: pointer;
  color: var(--text-main);
  transition: color 0.2s;
}

.radio-label:hover {
  color: var(--primary);
}

.search-form {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(180px, 1fr));
  gap: 20px;
  align-items: end;
}

.input-group {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.input-group label {
  font-size: 0.85rem;
  font-weight: 600;
  color: var(--text-muted);
  text-transform: uppercase;
  letter-spacing: 0.5px;
}

.input-wrapper {
  position: relative;
  display: flex;
  align-items: center;
}

.input-wrapper .icon {
  position: absolute;
  left: 12px;
  font-size: 1.2rem;
  pointer-events: none;
}

.input-wrapper input,
.input-wrapper select {
  width: 100%;
  padding-left: 40px; /* Espacio para el icono */
  background: white;
  cursor: pointer;
}

.input-wrapper input[type="date"] {
  padding-left: 16px;
}

.search-btn {
  background: var(--primary);
  color: white;
  font-weight: 600;
  font-size: 1rem;
  padding: 14px 24px;
  border-radius: 8px;
  height: 46px; /* Para igualar la altura de los inputs */
  display: flex;
  align-items: center;
  justify-content: center;
  box-shadow: 0 4px 14px 0 rgba(99, 102, 241, 0.39);
}

.search-btn:hover {
  background: var(--primary-hover);
  transform: translateY(-2px);
  box-shadow: 0 6px 20px rgba(99, 102, 241, 0.23);
}

.search-btn:active {
  transform: translateY(0);
}

/* Ajustes responsive */
@media (max-width: 768px) {
  .search-form {
    grid-template-columns: 1fr;
  }
}

@keyframes fly {
  0% { transform: translateX(-20px) translateY(10px) rotate(-10deg); opacity: 0; }
  50% { opacity: 1; }
  100% { transform: translateX(20px) translateY(-10px) rotate(10deg); opacity: 0; }
}
</style>
