<script setup>
import { ref, computed } from 'vue'
import { useRouter } from 'vue-router'
import FlightSearchForm from '../components/FlightSearchForm.vue'
import FlightCard from '../components/FlightCard.vue'
import { useFlightStore } from '../stores/index'
import { useAuthStore } from '../stores/auth'

const flightStore = useFlightStore()
const authStore = useAuthStore()
const router = useRouter()

const maxPrice = ref(1500)
const stopFilters = ref({
  direct: true,
  one: true,
  multiple: true
})
const showLoginModal = ref(false)

const filteredFlights = computed(() => {
  return flightStore.flights.filter(flight => {
    // 1. Filtro de precio
    const matchesPrice = flight.precio_base <= maxPrice.value
    
    // 2. Filtro de escalas
    const numStops = flight._escalas || 0
    let matchesStops = false
    
    if (numStops === 0 && stopFilters.value.direct) matchesStops = true
    if (numStops === 1 && stopFilters.value.one) matchesStops = true
    if (numStops >= 2 && stopFilters.value.multiple) matchesStops = true
    
    return matchesPrice && matchesStops
  })
})

const hasSearched = computed(() => {
  return flightStore.flights.length > 0 || flightStore.isLoading || flightStore.error
})

const handleFlightSelection = (flight) => {
  if (!authStore.isAuthenticated) {
    showLoginModal.value = true
    return
  }
  
  flightStore.selectFlight(flight)
  router.push('/booking')
}

const goToLogin = () => {
  router.push({ path: '/auth', query: { tab: 'login' } })
}

const goToRegister = () => {
  router.push({ path: '/auth', query: { tab: 'register' } })
}
</script>

<template>
  <div class="home-container">
    <!-- Hero Section con la imagen generada -->
    <header class="hero" :class="{ 'hero-compact': hasSearched }">
      <div class="hero-overlay"></div>
      <div class="hero-content">
        <h1 v-if="!hasSearched">Descubre el mundo con nosotros</h1>
        <p v-if="!hasSearched">Reserva los mejores vuelos a los precios más bajos y viaja a donde siempre soñaste.</p>
        
        <!-- Aquí integramos el buscador -->
        <FlightSearchForm />
      </div>
    </header>

    <!-- Sección de contenido condicional -->
    
    <!-- Sección de destinos eliminada por falta de funcionalidad en el contrato -->
    <div v-if="!hasSearched" class="welcome-message container">
      <div class="welcome-card shadow-glass">
        <h3>Bienvenido a VuelosPro</h3>
        <p>Usa el buscador superior para encontrar las mejores rutas y ofertas reales directamente desde nuestra base de datos.</p>
      </div>
    </div>

    <!-- 2. Mostrar resultados si SE HA BUSCADO -->
    <div v-else class="results-content container">
      <!-- Barra lateral de Filtros -->
      <aside class="filters">
        <h3>Filtros</h3>
        
        <div class="filter-group">
          <h4>Escalas</h4>
          <label class="checkbox-label"><input type="checkbox" v-model="stopFilters.direct"> Directo</label>
          <label class="checkbox-label"><input type="checkbox" v-model="stopFilters.one"> 1 Escala</label>
          <label class="checkbox-label"><input type="checkbox" v-model="stopFilters.multiple"> 2+ Escalas</label>
        </div>

        <div class="filter-group">
          <h4>Precio Máximo: ${{ maxPrice }}</h4>
          <input type="range" min="300" max="2000" step="10" v-model="maxPrice" class="price-slider">
          <div class="price-range-labels">
            <span>$300</span>
            <span>$2000</span>
          </div>
        </div>
      </aside>

      <!-- Lista de Vuelos -->
      <main class="flights-list">
        <div class="results-info" v-if="!flightStore.isLoading && !flightStore.error">
          <h2>Se encontraron {{ filteredFlights.length }} vuelos</h2>
          <div class="sort-wrapper">
            <label>Ordenar por:</label>
            <select class="sort-select">
              <option>Precio: Menor a Mayor</option>
              <option>Duración: Más corto</option>
              <option>Hora de salida</option>
            </select>
          </div>
        </div>

        <!-- Banner de Error o Advertencia (Ej. si la API falla) -->
        <div v-if="flightStore.error" class="error-banner">
          ⚠️ {{ flightStore.error }}
        </div>

        <!-- Estado de Carga -->
        <div v-if="flightStore.isLoading" class="loading-state">
          <div class="spinner">✈️</div>
          <p>Buscando las mejores opciones para tu viaje...</p>
        </div>

        <!-- Resultados -->
        <div v-else-if="filteredFlights.length > 0" class="cards-wrapper">
          <FlightCard 
            v-for="flight in filteredFlights" 
            :key="flight.id_vuelo" 
            :flight="flight" 
            @select="handleFlightSelection"
          />
        </div>
        
        <!-- Estado sin resultados por filtros -->
        <div v-else-if="flightStore.flights.length > 0" class="loading-state">
          <p>No hay vuelos que coincidan con el precio máximo de ${{ maxPrice }}.</p>
        </div>
      </main>
    </div>

    <!-- Modal de Autenticación Requerida -->
    <div v-if="showLoginModal" class="modal-overlay" @click.self="showLoginModal = false">
      <div class="modal-content">
        <span class="icon-warning">🔒</span>
        <h3>Inicio de sesión requerido</h3>
        <p>Para continuar con la reserva de este vuelo, es necesario que inicies sesión o crees una cuenta.</p>
        <div class="modal-actions">
          <button @click="goToLogin" class="btn-primary">Iniciar Sesión</button>
          <button @click="goToRegister" class="btn-secondary">Crear Cuenta</button>
        </div>
        <button type="button" class="btn-close" @click="showLoginModal = false">Cancelar</button>
      </div>
    </div>
  </div>
</template>

<style scoped>
.home-container {
  width: 100%;
  min-height: 100vh;
  background-color: var(--bg-color);
  padding-bottom: 60px;
}

.hero {
  position: relative;
  width: 100%;
  height: 80vh;
  min-height: 600px;
  background-image: url('../assets/hero-bg.png');
  background-size: cover;
  background-position: center;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: all 0.5s ease;
}

.hero.hero-compact {
  height: 40vh;
  min-height: 350px;
  align-items: flex-end;
  padding-bottom: 40px;
}

.hero-overlay {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: linear-gradient(to bottom, rgba(0,0,0,0.5) 0%, rgba(0,0,0,0.2) 40%, rgba(0,0,0,0.7) 100%);
  z-index: 1;
}

.hero-content {
  position: relative;
  z-index: 2;
  text-align: center;
  color: white;
  width: 100%;
  max-width: 1200px;
  padding: 0 24px;
  transition: all 0.5s ease;
}

.hero-content h1 {
  font-size: 3.5rem;
  margin-bottom: 16px;
  text-shadow: 0 2px 10px rgba(0,0,0,0.5);
  font-family: var(--font-heading);
}

.hero-content p {
  font-size: 1.25rem;
  margin-bottom: 48px;
  text-shadow: 0 1px 5px rgba(0,0,0,0.5);
  opacity: 0.9;
}

/* Mensaje de bienvenida */
.welcome-message {
  padding: 80px 24px;
  text-align: center;
}

.welcome-card {
  background: white;
  padding: 60px;
  border-radius: 32px;
  max-width: 800px;
  margin: 0 auto;
}

.welcome-card h3 {
  font-size: 2.5rem;
  color: var(--text-main);
  margin-bottom: 16px;
}

.welcome-card p {
  font-size: 1.2rem;
  color: var(--text-muted);
}

/* Estilos de Resultados y Filtros */
.container {
  max-width: 1200px;
  margin: 0 auto;
}

.results-content {
  display: grid;
  grid-template-columns: 280px 1fr;
  gap: 32px;
  padding: 40px 20px;
  animation: fadeIn 0.5s ease;
}

@keyframes fadeIn {
  from { opacity: 0; transform: translateY(20px); }
  to { opacity: 1; transform: translateY(0); }
}

.filters {
  background: white;
  border-radius: var(--radius-md);
  padding: 24px;
  border: 1px solid #e2e8f0;
  box-shadow: var(--shadow-sm);
  height: fit-content;
  position: sticky;
  top: 24px;
}

.filters h3 {
  font-size: 1.25rem;
  margin-bottom: 24px;
  padding-bottom: 12px;
  border-bottom: 1px solid #e2e8f0;
  color: var(--text-main);
}

.filter-group {
  margin-bottom: 24px;
}

.filter-group h4 {
  font-size: 0.95rem;
  color: var(--text-main);
  margin-bottom: 12px;
}

.checkbox-label {
  display: flex;
  align-items: center;
  gap: 8px;
  margin-bottom: 8px;
  color: var(--text-muted);
  font-size: 0.95rem;
  cursor: pointer;
  transition: color 0.2s;
}

.checkbox-label:hover {
  color: var(--primary);
}

.checkbox-label input[type="checkbox"] {
  width: 16px;
  height: 16px;
  accent-color: var(--primary);
}

.price-slider {
  width: 100%;
  margin-bottom: 8px;
  accent-color: var(--primary);
}

.price-range-labels {
  display: flex;
  justify-content: space-between;
  font-size: 0.85rem;
  font-weight: 500;
  color: var(--text-muted);
}

.results-info {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 24px;
}

.results-info h2 {
  font-size: 1.5rem;
  color: var(--text-main);
}

.sort-wrapper {
  display: flex;
  align-items: center;
  gap: 12px;
}

.sort-wrapper label {
  font-size: 0.9rem;
  color: var(--text-muted);
  font-weight: 500;
}

.sort-select {
  padding: 10px 16px;
  border-radius: 8px;
  border: 1px solid #cbd5e1;
  background: white;
  font-weight: 500;
  color: var(--text-main);
  cursor: pointer;
}

.cards-wrapper {
  display: flex;
  flex-direction: column;
  gap: 16px;
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

.loading-state {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: 60px 0;
  color: var(--text-muted);
}

.spinner {
  font-size: 3rem;
  animation: fly 2s infinite ease-in-out;
  margin-bottom: 16px;
}

@keyframes fly {
  0% { transform: translateX(-20px) translateY(10px) rotate(-10deg); opacity: 0; }
  50% { opacity: 1; }
  100% { transform: translateX(20px) translateY(-10px) rotate(10deg); opacity: 0; }
}

@media (max-width: 992px) {
  .results-content {
    grid-template-columns: 1fr;
  }
  
  .filters {
    position: relative;
    top: 0;
  }
}

@media (max-width: 768px) {
  .hero-content h1 {
    font-size: 2.5rem;
  }
  .results-info {
    flex-direction: column;
    align-items: flex-start;
    gap: 16px;
  }
}

/* Modal Styles */
.modal-overlay {
  position: fixed;
  top: 0; left: 0; width: 100vw; height: 100vh;
  background: rgba(0,0,0,0.4);
  backdrop-filter: blur(8px);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 2000;
}

.modal-content {
  background: white;
  padding: 40px;
  border-radius: 24px;
  text-align: center;
  max-width: 420px;
  width: 90%;
  box-shadow: 0 25px 50px -12px rgba(0, 0, 0, 0.25);
  display: flex;
  flex-direction: column;
  gap: 20px;
  animation: modalPop 0.4s cubic-bezier(0.175, 0.885, 0.32, 1.275);
}

@keyframes modalPop {
  0% { transform: scale(0.9); opacity: 0; }
  100% { transform: scale(1); opacity: 1; }
}

.icon-warning { 
  font-size: 4rem;
  margin-bottom: 10px;
}

.modal-content h3 { 
  color: #1e293b; 
  font-size: 1.75rem; 
  margin: 0;
  font-weight: 700;
}

.modal-content p { 
  color: #64748b; 
  font-size: 1.1rem; 
  line-height: 1.6; 
  margin: 0; 
}

.modal-actions { 
  display: flex; 
  flex-direction: column;
  gap: 12px; 
  margin-top: 10px; 
}

.btn-primary { 
  width: 100%; 
  background: #2563eb; 
  color: white; 
  border: none; 
  padding: 14px; 
  border-radius: 12px; 
  font-weight: 700; 
  font-size: 1.05rem;
  cursor: pointer; 
  transition: all 0.3s; 
  box-shadow: 0 4px 14px 0 rgba(37, 99, 235, 0.39);
}

.btn-primary:hover { 
  background: #1d4ed8; 
  transform: translateY(-2px);
  box-shadow: 0 6px 20px rgba(37, 99, 235, 0.23);
}

.btn-secondary { 
  width: 100%; 
  background: #f1f5f9; 
  color: #475569; 
  border: none; 
  padding: 14px; 
  border-radius: 12px; 
  font-weight: 700; 
  font-size: 1.05rem;
  cursor: pointer; 
  transition: all 0.3s; 
}

.btn-secondary:hover { 
  background: #e2e8f0; 
}

.btn-close { 
  margin-top: 5px; 
  background: transparent; 
  border: none; 
  color: #94a3b8; 
  font-weight: 600; 
  cursor: pointer; 
  font-size: 0.95rem;
}

.btn-close:hover { 
  color: #64748b; 
  text-decoration: underline; 
}
</style>
