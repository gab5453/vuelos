import apiClient from './api'

// ==========================================
// AUDITORÍA
// ==========================================

export const getAuditoria = async (filtros = {}) => {
  try {
    const params = new URLSearchParams()
    
    // Filtros opcionales permitidos por el contrato
    if (filtros.tabla) params.append('tabla', filtros.tabla)
    if (filtros.operacion) params.append('operacion', filtros.operacion)
    if (filtros.usuario) params.append('usuario', filtros.usuario)
    if (filtros.fecha_inicio) params.append('fecha_inicio', filtros.fecha_inicio)
    if (filtros.fecha_fin) params.append('fecha_fin', filtros.fecha_fin)
    
    // También soporta paginación estándar (page, page_size)
    if (filtros.page) params.append('page', filtros.page)
    if (filtros.page_size) params.append('page_size', filtros.page_size)

    const response = await apiClient.get(`/auditoria?${params.toString()}`)
    return response.data
  } catch (error) {
    console.error('Error obteniendo registros de auditoría:', error)
    throw error
  }
}
