import { defineStore } from 'pinia'
import { buscarVuelos } from '../services/vuelos.service'

export const useFlightStore = defineStore('flight', {
  state: () => ({
    searchParams: null,
    flights: [],
    isLoading: false,
    error: null,
    selectedFlight: null
  }),
  actions: {
    async searchFlights(params) {
      this.isLoading = true
      this.error = null
      this.searchParams = params
      
      try {
        // Llama al microservicio real
        const response = await buscarVuelos(params)
        this.flights = response.data?.items || []
      } catch (err) {
        console.error('Error al buscar vuelos:', err)
        this.error = 'No se pudieron cargar los vuelos. Por favor, intente más tarde.'
        this.flights = []
      } finally {
        this.isLoading = false
      }
    },
    
    selectFlight(flight) {
      this.selectedFlight = flight
    }
  }
})
