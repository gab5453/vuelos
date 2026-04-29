import { defineStore } from 'pinia'
import { login as apiLogin, register as apiRegister } from '../services/auth.service'

export const useAuthStore = defineStore('auth', {
  state: () => ({
    user: JSON.parse(localStorage.getItem('user_data')) || null,
    token: localStorage.getItem('jwt_token') || null,
    isLoading: false,
    error: null
  }),
  getters: {
    isAuthenticated: (state) => !!state.token
  },
  actions: {
    async login(credentials) {
      this.isLoading = true
      this.error = null
      try {
        const response = await apiLogin(credentials)
        this.token = response.token
        this.user = response.user
        
        localStorage.setItem('jwt_token', this.token)
        localStorage.setItem('user_data', JSON.stringify(this.user))
      } catch (error) {
        this.error = error.response?.data?.mensaje || 'Error al iniciar sesion. Verifica tus credenciales.'
        throw error
      } finally {
        this.isLoading = false
      }
    },
    async register(userData) {
      this.isLoading = true
      this.error = null
      try {
        const response = await apiRegister(userData)
        if (response.token) {
          this.token = response.token
          this.user = response.user
          localStorage.setItem('jwt_token', this.token)
          localStorage.setItem('user_data', JSON.stringify(this.user))
        }
        return response
      } catch (error) {
        this.error = error.response?.data?.mensaje || 'Error al crear la cuenta.'
        throw error
      } finally {
        this.isLoading = false
      }
    },
    updateUserData(newData) {
      // Fusionar datos nuevos con los existentes para no perder el rol u otros campos internos
      this.user = { ...this.user, ...newData }
      localStorage.setItem('user_data', JSON.stringify(this.user))
    },
    logout() {
      this.user = null
      this.token = null
      localStorage.removeItem('jwt_token')
      localStorage.removeItem('user_data')
    }
  }
})
