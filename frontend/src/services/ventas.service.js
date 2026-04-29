import apiClient from './api'

// ==========================================
// RESERVAS
// ==========================================

export const getReservas = async (filtros = {}) => {
  try {
    const params = new URLSearchParams()
    if (filtros.estado) params.append('estado', filtros.estado)
    if (filtros.id_cliente) params.append('id_cliente', filtros.id_cliente)
    
    const response = await apiClient.get(`/reservas?${params.toString()}`)
    return response.data
  } catch (error) {
    console.error('Error obteniendo reservas:', error)
    throw error
  }
}

export const crearReserva = async (reservaData) => {
  try {
    const response = await apiClient.post('/reservas', reservaData)
    return response.data
  } catch (error) {
    console.error('Error creando reserva:', error)
    throw error
  }
}

export const getReserva = async (idReserva) => {
  try {
    const response = await apiClient.get(`/reservas/${idReserva}`)
    return response.data
  } catch (error) {
    console.error(`Error obteniendo reserva ${idReserva}:`, error)
    throw error
  }
}

export const cambiarEstadoReserva = async (idReserva, estado) => {
  try {
    const response = await apiClient.patch(`/reservas/${idReserva}/estado`, { estado })
    return response.data
  } catch (error) {
    console.error(`Error cambiando estado de la reserva ${idReserva}:`, error)
    throw error
  }
}

// ==========================================
// FACTURAS
// ==========================================

export const getFacturas = async (filtros = {}) => {
  try {
    const params = new URLSearchParams()
    if (filtros.estado) params.append('estado', filtros.estado)
    if (filtros.id_cliente) params.append('id_cliente', filtros.id_cliente)
    
    const response = await apiClient.get(`/facturas?${params.toString()}`)
    return response.data
  } catch (error) {
    console.error('Error obteniendo facturas:', error)
    throw error
  }
}

export const emitirFactura = async (facturaData) => {
  try {
    const response = await apiClient.post('/facturas', facturaData)
    return response.data
  } catch (error) {
    console.error('Error emitiendo factura:', error)
    throw error
  }
}

export const getFactura = async (idFactura) => {
  try {
    const response = await apiClient.get(`/facturas/${idFactura}`)
    return response.data
  } catch (error) {
    console.error(`Error obteniendo factura ${idFactura}:`, error)
    throw error
  }
}

export const anularFactura = async (idFactura) => {
  try {
    const response = await apiClient.patch(`/facturas/${idFactura}/anular`)
    return response.data
  } catch (error) {
    console.error(`Error anulando factura ${idFactura}:`, error)
    throw error
  }
}

// ==========================================
// BOLETOS
// ==========================================

export const getBoletos = async (filtros = {}) => {
  try {
    const params = new URLSearchParams()
    if (filtros.id_vuelo) params.append('id_vuelo', filtros.id_vuelo)
    if (filtros.id_reserva) params.append('id_reserva', filtros.id_reserva)
    if (filtros.estado) params.append('estado', filtros.estado)
    
    const response = await apiClient.get(`/boletos?${params.toString()}`)
    return response.data
  } catch (error) {
    console.error('Error obteniendo boletos:', error)
    throw error
  }
}

export const emitirBoleto = async (boletoData) => {
  try {
    const response = await apiClient.post('/boletos', boletoData)
    return response.data
  } catch (error) {
    console.error('Error emitiendo boleto:', error)
    throw error
  }
}

export const getBoleto = async (idBoleto) => {
  try {
    const response = await apiClient.get(`/boletos/${idBoleto}`)
    return response.data
  } catch (error) {
    console.error(`Error obteniendo boleto ${idBoleto}:`, error)
    throw error
  }
}

// ==========================================
// EQUIPAJE
// ==========================================

export const getEquipajeBoleto = async (idBoleto) => {
  try {
    const response = await apiClient.get(`/boletos/${idBoleto}/equipaje`)
    return response.data
  } catch (error) {
    console.error(`Error obteniendo equipaje del boleto ${idBoleto}:`, error)
    throw error
  }
}

export const registrarEquipaje = async (idBoleto, equipajeData) => {
  try {
    const response = await apiClient.post(`/boletos/${idBoleto}/equipaje`, equipajeData)
    return response.data
  } catch (error) {
    console.error(`Error registrando equipaje para el boleto ${idBoleto}:`, error)
    throw error
  }
}

export const actualizarEstadoEquipaje = async (idBoleto, idEquipaje, estado) => {
  try {
    const response = await apiClient.patch(`/boletos/${idBoleto}/equipaje/${idEquipaje}/estado`, { estado })
    return response.data
  } catch (error) {
    console.error(`Error actualizando estado del equipaje ${idEquipaje} para el boleto ${idBoleto}:`, error)
    throw error
  }
}
