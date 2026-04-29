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
        const response = await buscarVuelos(params)
        
        // 1. Extraemos el arreglo de la respuesta de forma segura
        const vuelosCrudos = response.data?.items || response.items || []

        // 2. TRADUCCIÓN: Pasamos de CamelCase (BD) a snake_case (Vue)
        this.flights = vuelosCrudos.map(flight => ({
          ...flight,
          id_vuelo: flight.idVuelo,
          numero_vuelo: flight.numeroVuelo,
          precio_base: flight.precioBase,
          fecha_hora_salida: flight.fechaHoraSalida,
          fecha_hora_llegada: flight.fechaHoraLlegada,
          duracion_min: flight.duracionMin,
          estado_vuelo: flight.estadoVuelo,
          origen: {
            ...flight.origen,
            id_aeropuerto: flight.origen?.idAeropuerto,
            codigo_iata: flight.origen?.codigoIata,
            nombre_ciudad: flight.origen?.nombreCiudad,
            nombre: flight.origen?.nombre
          },
          destino: {
            ...flight.destino,
            id_aeropuerto: flight.destino?.idAeropuerto,
            codigo_iata: flight.destino?.codigoIata,
            nombre_ciudad: flight.destino?.nombreCiudad,
            nombre: flight.destino?.nombre
          }
        }))

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