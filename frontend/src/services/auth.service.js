import apiClient from './api'

export const login = async (credentials) => {
  try {
    // Conectando al endpoint de autenticación del backend (estándar esperado)
    const response = await apiClient.post('/auth/login', credentials)
    return response.data
  } catch (error) {
    console.error('Error en login:', error)
    throw error
  }
}

export const register = async (userData) => {
  try {
    const response = await apiClient.post('/auth/registro', userData)
    return response.data
  } catch (error) {
    console.error('Error en registro:', error)
    throw error
  }
}
