import apiClient from './api'

// ==========================================
// MÉTODOS DE PAGO
// ==========================================

export const getMetodosPago = async (idCliente) => {
  try {
    const response = await apiClient.get(`/clientes/${idCliente}/metodos-pago`)
    return response.data
  } catch (error) {
    console.error(`Error obteniendo métodos de pago para el cliente ${idCliente}:`, error)
    throw error
  }
}

export const registrarMetodoPago = async (idCliente, metodoPagoData) => {
  try {
    const response = await apiClient.post(`/clientes/${idCliente}/metodos-pago`, metodoPagoData)
    return response.data
  } catch (error) {
    console.error(`Error registrando método de pago para el cliente ${idCliente}:`, error)
    throw error
  }
}

export const eliminarMetodoPago = async (idCliente, idMetodo) => {
  try {
    const response = await apiClient.delete(`/clientes/${idCliente}/metodos-pago/${idMetodo}`)
    return response.data
  } catch (error) {
    console.error(`Error eliminando método de pago ${idMetodo} del cliente ${idCliente}:`, error)
    throw error
  }
}
