import apiClient from './api'

// ==========================================
// SEGURIDAD / USUARIOS
// ==========================================

export const getUsuarios = async () => {
  try {
    const response = await apiClient.get('/usuarios')
    return response.data
  } catch (error) {
    console.error('Error obteniendo usuarios:', error)
    throw error
  }
}

export const crearUsuario = async (usuarioData) => {
  try {
    const response = await apiClient.post('/usuarios', usuarioData)
    return response.data
  } catch (error) {
    console.error('Error creando usuario:', error)
    throw error
  }
}

// ==========================================
// SEGURIDAD / ROLES
// ==========================================

export const getRolesUsuario = async (idUsuario) => {
  try {
    const response = await apiClient.get(`/usuarios/${idUsuario}/roles`)
    return response.data
  } catch (error) {
    console.error(`Error obteniendo roles del usuario ${idUsuario}:`, error)
    throw error
  }
}

export const asignarRolUsuario = async (idUsuario, rolData) => {
  try {
    // El rolData podría ser un objeto con el ID del rol u otros datos requeridos por la API
    const response = await apiClient.post(`/usuarios/${idUsuario}/roles`, rolData)
    return response.data
  } catch (error) {
    console.error(`Error asignando rol al usuario ${idUsuario}:`, error)
    throw error
  }
}

export const quitarRolUsuario = async (idUsuario, idRol) => {
  try {
    const response = await apiClient.delete(`/usuarios/${idUsuario}/roles/${idRol}`)
    return response.data
  } catch (error) {
    console.error(`Error quitando el rol ${idRol} al usuario ${idUsuario}:`, error)
    throw error
  }
}
