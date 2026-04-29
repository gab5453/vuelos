import apiClient from './api'

// ==========================================
// CLIENTES (CRM)
// ==========================================

/**
 * Obtiene una lista paginada de clientes segun el contrato 6.1.
 * @param {Object} filtros - Filtros opcionales (numero_identificacion, correo, estado, page, page_size)
 */
export const getClientes = async (filtros = {}) => {
  try {
    const params = new URLSearchParams()
    
    // Mapeo de filtros segun el contrato v1.0
    if (filtros.numero_identificacion) params.append('numero_identificacion', filtros.numero_identificacion)
    if (filtros.correo) params.append('correo', filtros.correo)
    if (filtros.estado) params.append('estado', filtros.estado)
    
    // Paginacion
    params.append('page', filtros.page || 1)
    params.append('page_size', filtros.page_size || 20)
    
    const response = await apiClient.get(`/clientes?${params.toString()}`)
    return response.data
  } catch (error) {
    console.error('Error obteniendo lista de clientes:', error)
    throw error
  }
}

/**
 * Crea un nuevo cliente segun el contrato 6.2.
 * @param {Object} clienteData - Datos del cliente en snake_case
 */
export const crearCliente = async (clienteData) => {
  try {
    // El objeto clienteData ya deberia venir en snake_case desde el componente/store
    const response = await apiClient.post('/clientes', clienteData)
    return response.data
  } catch (error) {
    console.error('Error al registrar cliente:', error)
    throw error
  }
}

/**
 * Obtiene el detalle de un cliente por su GUID.
 */
export const getClienteByGuid = async (guidCliente) => {
  try {
    const response = await apiClient.get(`/clientes/${guidCliente}`)
    return response.data
  } catch (error) {
    console.error(`Error obteniendo cliente por GUID ${guidCliente}:`, error)
    throw error
  }
}

/**
 * Obtiene un cliente por su identificacion.
 */
export const getClienteByIdentificacion = async (tipo, numero) => {
  try {
    const response = await apiClient.get(`/clientes/identificacion/${tipo}/${numero}`)
    return response.data
  } catch (error) {
    console.error(`Error obteniendo cliente ${tipo}-${numero}:`, error)
    throw error
  }
}

/**
 * Actualiza un cliente existente.
 */
export const actualizarCliente = async (guidCliente, clienteData) => {
  try {
    const response = await apiClient.put(`/clientes/${guidCliente}`, clienteData)
    return response.data
  } catch (error) {
    console.error(`Error actualizando cliente ${guidCliente}:`, error)
    throw error
  }
}

/**
 * Cambia el estado de un cliente.
 */
export const cambiarEstadoCliente = async (guidCliente, nuevoEstado) => {
  try {
    const response = await apiClient.patch(`/clientes/${guidCliente}/estado/${nuevoEstado}`)
    return response.data
  } catch (error) {
    console.error(`Error cambiando estado de cliente ${guidCliente}:`, error)
    throw error
  }
}

/**
 * Eliminacion logica de un cliente.
 */
export const eliminarCliente = async (guidCliente) => {
  try {
    const response = await apiClient.delete(`/clientes/${guidCliente}`)
    return response.data
  } catch (error) {
    console.error(`Error eliminando cliente ${guidCliente}:`, error)
    throw error
  }
}

// ==========================================
// PASAJEROS
// ==========================================

export const getPasajeros = async (filtros = {}) => {
  try {
    const params = new URLSearchParams()
    if (filtros.id_cliente) params.append('id_cliente', filtros.id_cliente)
    
    const response = await apiClient.get(`/pasajeros?${params.toString()}`)
    return response.data
  } catch (error) {
    console.error('Error obteniendo pasajeros:', error)
    throw error
  }
}

export const crearPasajero = async (pasajeroData) => {
  try {
    const response = await apiClient.post('/pasajeros', pasajeroData)
    return response.data
  } catch (error) {
    console.error('Error creando pasajero:', error)
    throw error
  }
}
