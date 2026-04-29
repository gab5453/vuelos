import axios from 'axios'

// Configuración base de Axios para las llamadas a tus Microservicios (.NET)
const apiClient = axios.create({
  // URL Base según el contrato de la API de Vuelos
  baseURL: 'https://api-vuelos-backend-eud6g4aqfgduhrfa.brazilsouth-01.azurewebsites.net/api',
  headers: {
    'Content-Type': 'application/json'
  }
})

apiClient.interceptors.request.use((config) => {
  const token = localStorage.getItem('jwt_token')
  if (token) {
    config.headers.Authorization = `Bearer ${token}`
  }
  return config
}, (error) => {
  return Promise.reject(error)
})

export default apiClient
