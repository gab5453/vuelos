import apiClient from './api'

export const buscarVuelos = async (filtros) => {
  try {
    // Parámetros ajustados al formato esperado por el backend en C# (PascalCase)
    const params = new URLSearchParams()
    if (filtros.origin) params.append('IdAeropuertoOrigen', filtros.origin)
    if (filtros.destination) params.append('IdAeropuertoDestino', filtros.destination)
    if (filtros.departureDate) params.append('FechaSalida', filtros.departureDate)
    if (filtros.estado) params.append('EstadoVuelo', filtros.estado || 'PROGRAMADO')

    const response = await apiClient.get(`/vuelos?${params.toString()}`)
    return response.data // El store ya maneja el desempaquetado de .data.items para búsquedas
  } catch (error) {
    console.error('Error llamando al API de vuelos:', error)
    throw error
  }
}

export const getVuelo = async (idVuelo) => {
  try {
    const response = await apiClient.get(`/vuelos/${idVuelo}`)
    return response.data.data
  } catch (error) {
    console.error(`Error obteniendo vuelo ${idVuelo}:`, error)
    throw error
  }
}

export const crearVuelo = async (vueloData) => {
  try {
    const response = await apiClient.post('/vuelos', vueloData)
    return response.data.data
  } catch (error) {
    console.error('Error creando vuelo:', error)
    throw error
  }
}

export const actualizarVuelo = async (idVuelo, vueloData) => {
  try {
    const response = await apiClient.put(`/vuelos/${idVuelo}`, vueloData)
    return response.data.data
  } catch (error) {
    console.error(`Error actualizando vuelo ${idVuelo}:`, error)
    throw error
  }
}

export const eliminarVuelo = async (idVuelo) => {
  try {
    const response = await apiClient.delete(`/vuelos/${idVuelo}`)
    return response.data
  } catch (error) {
    console.error(`Error eliminando vuelo ${idVuelo}:`, error)
    throw error
  }
}

export const cambiarEstadoVuelo = async (idVuelo, estado) => {
  try {
    const response = await apiClient.patch(`/vuelos/${idVuelo}/estado`, { estado })
    return response.data
  } catch (error) {
    console.error(`Error cambiando estado del vuelo ${idVuelo}:`, error)
    throw error
  }
}

export const getEscalas = async (idVuelo) => {
  try {
    const response = await apiClient.get(`/vuelos/${idVuelo}/escalas`)
    return response.data.data
  } catch (error) {
    console.error(`Error obteniendo escalas del vuelo ${idVuelo}:`, error)
    throw error
  }
}

export const getEscala = async (idVuelo, idEscala) => {
  try {
    const response = await apiClient.get(`/vuelos/${idVuelo}/escalas/${idEscala}`)
    return response.data
  } catch (error) {
    console.error(`Error obteniendo escala ${idEscala} del vuelo ${idVuelo}:`, error)
    throw error
  }
}

export const agregarEscala = async (idVuelo, escalaData) => {
  try {
    const response = await apiClient.post(`/vuelos/${idVuelo}/escalas`, escalaData)
    return response.data
  } catch (error) {
    console.error(`Error agregando escala al vuelo ${idVuelo}:`, error)
    throw error
  }
}

export const eliminarEscala = async (idVuelo, idEscala) => {
  try {
    const response = await apiClient.delete(`/vuelos/${idVuelo}/escalas/${idEscala}`)
    return response.data
  } catch (error) {
    console.error(`Error eliminando escala ${idEscala} del vuelo ${idVuelo}:`, error)
    throw error
  }
}

export const getAsientos = async (idVuelo, filtros = {}) => {
  try {
    const params = new URLSearchParams()
    if (filtros.disponible !== undefined) params.append('disponible', filtros.disponible)
    if (filtros.clase) params.append('clase', filtros.clase)

    const response = await apiClient.get(`/vuelos/${idVuelo}/asientos?${params.toString()}`)
    return response.data.data
  } catch (error) {
    console.error(`Error obteniendo asientos del vuelo ${idVuelo}:`, error)
    throw error
  }
}

export const getAsiento = async (idVuelo, idAsiento) => {
  try {
    const response = await apiClient.get(`/vuelos/${idVuelo}/asientos/${idAsiento}`)
    return response.data
  } catch (error) {
    console.error(`Error obteniendo asiento ${idAsiento} del vuelo ${idVuelo}:`, error)
    throw error
  }
}

export const crearAsiento = async (idVuelo, asientoData) => {
  try {
    const response = await apiClient.post(`/vuelos/${idVuelo}/asientos`, asientoData)
    return response.data
  } catch (error) {
    console.error(`Error creando asiento para el vuelo ${idVuelo}:`, error)
    throw error
  }
}

export const actualizarDisponibilidadAsiento = async (idVuelo, idAsiento, disponibilidadData) => {
  try {
    const response = await apiClient.patch(`/vuelos/${idVuelo}/asientos/${idAsiento}`, disponibilidadData)
    return response.data
  } catch (error) {
    console.error(`Error actualizando asiento ${idAsiento} del vuelo ${idVuelo}:`, error)
    throw error
  }
}