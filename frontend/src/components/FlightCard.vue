<script setup>
import { computed } from 'vue'
import { useRouter } from 'vue-router'
import { useFlightStore } from '../stores/index'

const props = defineProps({
  flight: {
    type: Object,
    required: true
  }
})

const emit = defineEmits(['select'])

const selectFlight = () => {
  emit('select', props.flight)
}

// Helpers para formatear los datos del esquema de Vuelo
const formatTime = (isoString) => {
  if (!isoString) return '--:--'
  const date = new Date(isoString)
  return date.toLocaleTimeString('es-ES', { hour: '2-digit', minute: '2-digit' })
}

const formatDuration = (minutes) => {
  if (!minutes) return '--h --m'
  const h = Math.floor(minutes / 60)
  const m = minutes % 60
  return `${h}h ${m}m`
}

const departureTime = computed(() => formatTime(props.flight.fecha_hora_salida))
const arrivalTime = computed(() => formatTime(props.flight.fecha_hora_llegada))
const duration = computed(() => formatDuration(props.flight.duracion_min))

</script>

<template>
  <div class="flight-card">
    <div class="airline-info">
      <div class="airline-logo">✈️</div>
      <span class="airline-name">{{ flight._aerolinea || 'Aerolínea XYZ' }}</span>
      <span class="flight-number">{{ flight.numero_vuelo }}</span>
    </div>
    
    <div class="flight-details">
      <div class="time-block">
        <span class="time">{{ departureTime }}</span>
        <span class="airport">{{ flight._origen_code || 'ORG' }}</span>
      </div>
      
      <div class="duration-block">
        <span class="duration">{{ duration }}</span>
        <div class="line">
          <div class="plane-icon">✈️</div>
        </div>
        <span class="stops" :class="{ direct: flight._escalas === 0 }">
          {{ flight._escalas === 0 ? 'Directo' : `${flight._escalas} Escala(s)` }}
        </span>
      </div>
      
      <div class="time-block">
        <span class="time">{{ arrivalTime }}</span>
        <span class="airport">{{ flight._destino_code || 'DST' }}</span>
      </div>
    </div>
    
    <div class="price-action">
      <div class="price-container">
        <span class="currency">$</span>
        <span class="price">{{ flight.precio_base }}</span>
      </div>
      <button class="select-btn" @click="selectFlight">Seleccionar</button>
    </div>
  </div>
</template>

<style scoped>
.flight-card {
  display: flex;
  align-items: center;
  justify-content: space-between;
  background: white;
  border-radius: var(--radius-md);
  padding: 24px;
  box-shadow: var(--shadow-sm);
  border: 1px solid #e2e8f0;
  transition: transform 0.2s ease, box-shadow 0.2s ease;
}

.flight-card:hover {
  transform: translateY(-2px);
  box-shadow: var(--shadow-md);
  border-color: #cbd5e1;
}

.airline-info {
  display: flex;
  flex-direction: column;
  align-items: center;
  width: 120px;
}

.airline-logo {
  font-size: 2rem;
  background: #f1f5f9;
  width: 48px;
  height: 48px;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 50%;
  margin-bottom: 8px;
}

.airline-name {
  font-size: 0.85rem;
  font-weight: 600;
  color: var(--text-muted);
  text-align: center;
}

.flight-number {
  font-size: 0.75rem;
  color: #94a3b8;
  margin-top: 4px;
}

.flight-details {
  display: flex;
  align-items: center;
  flex: 1;
  justify-content: center;
  gap: 40px;
  padding: 0 40px;
}

.time-block {
  display: flex;
  flex-direction: column;
  align-items: center;
}

.time {
  font-size: 1.5rem;
  font-weight: 700;
  color: var(--text-main);
}

.airport {
  font-size: 0.9rem;
  font-weight: 500;
  color: var(--text-muted);
}

.duration-block {
  display: flex;
  flex-direction: column;
  align-items: center;
  width: 140px;
  gap: 14px;
}

.duration {
  font-size: 0.85rem;
  color: var(--text-muted);
  margin-bottom: 0;
}

.line {
  width: 100%;
  height: 1px;
  background: #cbd5e1;
  position: relative;
  display: flex;
  justify-content: center;
  align-items: center;
}

.plane-icon {
  position: absolute;
  font-size: 0.9rem;
  background: white;
  padding: 0 4px;
  z-index: 1;
}

.stops {
  font-size: 0.8rem;
  font-weight: 600;
  color: #f59e0b; /* Naranja para escalas */
}

.stops.direct {
  color: #10b981; /* Verde para directo */
}

.price-action {
  display: flex;
  flex-direction: column;
  align-items: flex-end;
  border-left: 1px solid #e2e8f0;
  padding-left: 32px;
  min-width: 160px;
}

.price-container {
  margin-bottom: 12px;
}

.currency {
  font-size: 1.2rem;
  font-weight: 600;
  color: var(--text-main);
  margin-right: 2px;
}

.price {
  font-size: 2rem;
  font-weight: 700;
  color: var(--text-main);
  letter-spacing: -1px;
}

.select-btn {
  background: var(--primary);
  color: white;
  font-weight: 600;
  padding: 12px 24px;
  border-radius: 8px;
  width: 100%;
  font-size: 1rem;
}

.select-btn:hover {
  background: var(--primary-hover);
}

@media (max-width: 768px) {
  .flight-card {
    flex-direction: column;
    gap: 20px;
  }
  
  .price-action {
    border-left: none;
    border-top: 1px solid #e2e8f0;
    padding-left: 0;
    padding-top: 20px;
    width: 100%;
    align-items: center;
  }
  
  .flight-details {
    gap: 16px;
    padding: 0;
    width: 100%;
    justify-content: space-between;
  }

  .duration-block {
    width: 100px;
  }
}
</style>
