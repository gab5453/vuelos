<script setup>
import { computed, ref, onMounted, onUnmounted } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { useAuthStore } from '../stores/auth'

const authStore = useAuthStore()
const router = useRouter()
const route = useRoute()

const isAuthenticated = computed(() => authStore.isAuthenticated)
const user = computed(() => authStore.user)
const isAuthPage = computed(() => route.path === '/auth')

const isMenuOpen = ref(false)

const toggleMenu = () => {
  isMenuOpen.value = !isMenuOpen.value
}

const closeMenu = (e) => {
  if (!e.target.closest('.user-profile-wrapper')) {
    isMenuOpen.value = false
  }
}

onMounted(() => {
  document.addEventListener('click', closeMenu)
})

onUnmounted(() => {
  document.removeEventListener('click', closeMenu)
})

const logout = () => {
  authStore.logout()
  isMenuOpen.value = false
  router.push('/')
}

const goToReservations = () => {
  isMenuOpen.value = false
  router.push('/reservas')
}

const goToProfile = () => {
  isMenuOpen.value = false
  router.push('/perfil')
}

const goToAdmin = () => {
  isMenuOpen.value = false
  router.push('/admin')
}
</script>

<template>
  <header class="app-header">
    <div class="logo" @click="router.push('/')">
      <span class="icon">✈️</span>
      <h1>Booking Vuelos</h1>
    </div>

    <nav class="auth-section">
      <template v-if="isAuthenticated && user">
        <div class="user-profile-wrapper">
          <div class="user-profile" @click="toggleMenu">
            <div class="avatar">
              {{ user.nombre ? user.nombre.charAt(0).toUpperCase() : 'U' }}
            </div>
            <span class="user-name">{{ user.nombre || 'Usuario' }}</span>
            <span class="dropdown-icon">▼</span>
          </div>
          
          <div v-if="isMenuOpen" class="dropdown-menu">
            <button v-if="user.rol === 'ADMINISTRADOR' || user.rol === 'ADMIN'" @click="goToAdmin" class="menu-item admin-item">
              <span class="icon-menu">âš™ï¸</span> Panel de Control
            </button>
            <button @click="goToProfile" class="menu-item">
              <span class="icon-menu">👤</span> Detalle de la cuenta
            </button>
            <button @click="goToReservations" class="menu-item">
              <span class="icon-menu">🎫</span> Mis reservas
            </button>
            <hr>
            <button @click="logout" class="menu-item logout-item">
              <span class="icon-menu">🚪</span> Cerrar Sesión
            </button>
          </div>
        </div>
      </template>
      <template v-else>
        <div class="auth-buttons" v-if="!isAuthPage">
          <button @click="router.push({ path: '/auth', query: { tab: 'login' } })" class="btn-login">
            Iniciar Sesión
          </button>
          <button @click="router.push({ path: '/auth', query: { tab: 'register' } })" class="btn-register">
            Registrarse
          </button>
        </div>
      </template>
    </nav>
  </header>
</template>

<style scoped>
.app-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1rem 2rem;
  background: rgba(255, 255, 255, 0.8);
  backdrop-filter: blur(12px);
  -webkit-backdrop-filter: blur(12px);
  border-bottom: 1px solid rgba(0, 0, 0, 0.05);
  position: sticky;
  top: 0;
  z-index: 100;
  box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.05);
}

.logo {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  cursor: pointer;
  transition: transform 0.2s ease;
}

.logo:hover {
  transform: scale(1.02);
}

.icon {
  font-size: 1.8rem;
}

h1 {
  font-size: 1.4rem;
  font-weight: 700;
  background: linear-gradient(135deg, #2563eb, #7c3aed);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  margin: 0;
}

.auth-section {
  display: flex;
  align-items: center;
}

.auth-buttons {
  display: flex;
  gap: 1rem;
}

button {
  font-family: inherit;
  font-size: 0.95rem;
  font-weight: 600;
  padding: 0.6rem 1.2rem;
  border-radius: 8px;
  cursor: pointer;
  transition: all 0.3s ease;
}

.btn-login {
  background: transparent;
  border: 2px solid #e2e8f0;
  color: #475569;
}

.btn-login:hover {
  border-color: #2563eb;
  color: #2563eb;
  background: rgba(37, 99, 235, 0.05);
}

.btn-register {
  background: linear-gradient(135deg, #2563eb, #3b82f6);
  border: none;
  color: white;
  box-shadow: 0 4px 14px 0 rgba(37, 99, 235, 0.39);
}

.btn-register:hover {
  transform: translateY(-2px);
  box-shadow: 0 6px 20px rgba(37, 99, 235, 0.4);
}

.user-profile-wrapper {
  position: relative;
}

.user-profile {
  display: flex;
  align-items: center;
  gap: 1rem;
  cursor: pointer;
  padding: 0.4rem 0.8rem;
  border-radius: 30px;
  transition: background 0.2s;
}

.user-profile:hover {
  background: rgba(0,0,0,0.05);
}

.dropdown-icon {
  font-size: 0.7rem;
  color: #64748b;
}

.avatar {
  width: 40px;
  height: 40px;
  border-radius: 50%;
  background: linear-gradient(135deg, #7c3aed, #ec4899);
  color: white;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: 700;
  font-size: 1.2rem;
  box-shadow: 0 4px 10px rgba(124, 58, 237, 0.3);
}

.user-name {
  font-weight: 600;
  color: #1e293b;
}

.dropdown-menu {
  position: absolute;
  top: calc(100% + 10px);
  right: 0;
  width: 260px;
  background: white;
  border-radius: 12px;
  box-shadow: 0 10px 25px rgba(0,0,0,0.1);
  border: 1px solid #e2e8f0;
  overflow: hidden;
  animation: dropdownPop 0.2s ease;
  z-index: 200;
}

@keyframes dropdownPop {
  0% { opacity: 0; transform: translateY(-10px); }
  100% { opacity: 1; transform: translateY(0); }
}

.dropdown-menu hr {
  margin: 0;
  border: none;
  border-top: 1px solid #e2e8f0;
}

.menu-item {
  width: 100%;
  display: flex;
  align-items: center;
  gap: 12px;
  padding: 14px 16px;
  border: none;
  background: transparent;
  color: #334155;
  font-size: 0.95rem;
  font-weight: 600;
  cursor: pointer;
  text-align: left;
  border-radius: 0;
  transition: background 0.2s;
}

.menu-item:hover {
  background: #f1f5f9;
}

.logout-item {
  color: #ef4444;
}

.logout-item:hover {
  background: #fef2f2;
}

.icon-menu {
  font-size: 1.1rem;
}

@media (max-width: 600px) {
  .user-name {
    display: none;
  }
}
</style>

