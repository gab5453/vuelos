import apiClient from './api'

// ==========================================
// PAISES
// ==========================================

export const getPaises = async () => {
  try {
    const response = await apiClient.get('/paises')
    return response.data.data
  } catch (error) {
    console.error('Error obteniendo países:', error)
    throw error
  }
}

export const getPais = async (idPais) => {
  try {
    const response = await apiClient.get(`/paises/${idPais}`)
    return response.data
  } catch (error) {
    console.error(`Error obteniendo país ${idPais}:`, error)
    throw error
  }
}

// ==========================================
// CIUDADES
// ==========================================

export const getCiudades = async (filtros = {}) => {
  try {
    const params = new URLSearchParams()
    if (filtros.id_pais) params.append('id_pais', filtros.id_pais)
    
    const response = await apiClient.get(`/ciudades?${params.toString()}`)
    return response.data.data
  } catch (error) {
    console.error('Error obteniendo ciudades:', error)
    throw error
  }
}

export const getCiudad = async (idCiudad) => {
  try {
    const response = await apiClient.get(`/ciudades/${idCiudad}`)
    return response.data
  } catch (error) {
    console.error(`Error obteniendo ciudad ${idCiudad}:`, error)
    throw error
  }
}

export const crearCiudad = async (ciudadData) => {
  try {
    const response = await apiClient.post('/ciudades', ciudadData)
    return response.data
  } catch (error) {
    console.error('Error creando ciudad:', error)
    throw error
  }
}

export const actualizarCiudad = async (idCiudad, ciudadData) => {
  try {
    const response = await apiClient.put(`/ciudades/${idCiudad}`, ciudadData)
    return response.data
  } catch (error) {
    console.error(`Error actualizando ciudad ${idCiudad}:`, error)
    throw error
  }
}

export const eliminarCiudad = async (idCiudad) => {
  try {
    const response = await apiClient.delete(`/ciudades/${idCiudad}`)
    return response.data
  } catch (error) {
    console.error(`Error eliminando ciudad ${idCiudad}:`, error)
    throw error
  }
}

// ==========================================
// AEROPUERTOS
// ==========================================

export const getAeropuertos = async () => {
  try {
    const response = await apiClient.get('/aeropuertos')
    return response.data.data?.items || response.data.data || []
  } catch (error) {
    console.error('Error obteniendo aeropuertos:', error)
    throw error
  }
}

export const getAeropuerto = async (idAeropuerto) => {
  try {
    const response = await apiClient.get(`/aeropuertos/${idAeropuerto}`)
    return response.data
  } catch (error) {
    console.error(`Error obteniendo aeropuerto ${idAeropuerto}:`, error)
    throw error
  }
}

export const crearAeropuerto = async (aeropuertoData) => {
  try {
    const response = await apiClient.post('/aeropuertos', aeropuertoData)
    return response.data
  } catch (error) {
    console.error('Error creando aeropuerto:', error)
    throw error
  }
}

export const actualizarAeropuerto = async (idAeropuerto, aeropuertoData) => {
  try {
    const response = await apiClient.put(`/aeropuertos/${idAeropuerto}`, aeropuertoData)
    return response.data
  } catch (error) {
    console.error(`Error actualizando aeropuerto ${idAeropuerto}:`, error)
    throw error
  }
}

export const eliminarAeropuerto = async (idAeropuerto) => {
  try {
    const response = await apiClient.delete(`/aeropuertos/${idAeropuerto}`)
    return response.data
  } catch (error) {
    console.error(`Error eliminando aeropuerto ${idAeropuerto}:`, error)
    throw error
  }
}
