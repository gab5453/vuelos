<script setup>
import { ref, onMounted } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { useAuthStore } from '../stores/auth'
import * as catalogosService from '../services/catalogos.service'

const router = useRouter()
const route = useRoute()
const authStore = useAuthStore()

const activeTab = ref('login')
const countries = ref([])

const loginForm = ref({
  email: '',
  password: ''
})

const registerForm = ref({
  nombres: '',
  apellidos: '',
  correo: '',
  password: '',
  tipo_id: 'CEDULA',
  identificacion: '',
  fecha_nacimiento: '',
  genero: '',
  id_pais_nacionalidad: null,
  nacionalidad: ''
})

onMounted(async () => {
  if (route.query.tab === 'register') {
    activeTab.value = 'register'
  }
  
  try {
    countries.value = await catalogosService.getPaises()
  } catch (e) {
    console.error('Error loading countries', e)
  }
})

const handleLogin = async () => {
  try {
    await authStore.login({
      email: loginForm.value.email,
      password: loginForm.value.password
    })
    router.push('/')
  } catch (error) {
    console.error('Error al iniciar sesion', error)
  }
}

const handleRegister = async () => {
  try {
    // Preparar datos para el contrato v1.0
    const payload = {
      nombres: registerForm.value.nombres,
      apellidos: registerForm.value.apellidos,
      correo: registerForm.value.correo,
      password: registerForm.value.password,
      tipo_identificacion: registerForm.value.tipo_id,
      numero_identificacion: registerForm.value.identificacion,
      fecha_nacimiento: registerForm.value.fecha_nacimiento,
      genero: registerForm.value.genero,
      id_pais_nacionalidad: registerForm.value.id_pais_nacionalidad,
      nacionalidad: registerForm.value.nacionalidad,
      estado: 'ACT'
    }

    await authStore.register(payload)
    router.push('/')
  } catch (error) {
    console.error('Error al registrarse', error)
  }
}

const toggleTab = (tab) => {
  activeTab.value = tab
  authStore.error = null
}
</script>

<template>
  <div class="auth-container">
    <div class="auth-card glass-panel" :class="{ 'wide-card': activeTab === 'register' }">
      
      <div class="tabs">
        <button 
          :class="['tab-btn', { active: activeTab === 'login' }]"
          @click="toggleTab('login')"
        >
          Iniciar Sesion
        </button>
        <button 
          :class="['tab-btn', { active: activeTab === 'register' }]"
          @click="toggleTab('register')"
        >
          Registrarse
        </button>
      </div>

      <div v-if="authStore.error" class="error-banner">
        {{ authStore.error }}
      </div>

      <!-- Login Form -->
      <form v-if="activeTab === 'login'" @submit.prevent="handleLogin" class="form-content fade-in">
        <h2 class="form-title">Bienvenido de nuevo</h2>
        <p class="form-subtitle">Ingresa tus credenciales para continuar</p>

        <div class="input-group">
          <label>Correo Electronico</label>
          <input type="email" v-model="loginForm.email" required placeholder="ejemplo@correo.com"/>
        </div>
        
        <div class="input-group">
          <label>Contraseña</label>
          <input type="password" v-model="loginForm.password" required placeholder="********"/>
        </div>

        <button type="submit" class="submit-btn" :disabled="authStore.isLoading">
          <span v-if="authStore.isLoading">Iniciando sesion...</span>
          <span v-else>Entrar</span>
        </button>
      </form>

      <!-- Register Form -->
      <form v-else @submit.prevent="handleRegister" class="form-content fade-in">
        <h2 class="form-title">Crea tu cuenta</h2>
        <p class="form-subtitle">Completa tu perfil para empezar a viajar</p>

        <div class="form-grid">
          <div class="grid-col">
            <div class="input-group">
              <label>Nombres</label>
              <input type="text" v-model="registerForm.nombres" required placeholder="Juan Pablo"/>
            </div>
            <div class="input-group">
              <label>Apellidos</label>
              <input type="text" v-model="registerForm.apellidos" required placeholder="Perez"/>
            </div>
            <div class="form-row">
              <div class="input-group">
                <label>Tipo Doc.</label>
                <select v-model="registerForm.tipo_id">
                  <option value="CEDULA">Cedula</option>
                  <option value="PASAPORTE">Pasaporte</option>
                </select>
              </div>
              <div class="input-group">
                <label>Nº Documento</label>
                <input type="text" v-model="registerForm.identificacion" required />
              </div>
            </div>
          </div>

          <div class="grid-col">
            <div class="form-row">
              <div class="input-group">
                <label>Fecha Nacimiento</label>
                <input 
                  type="date" 
                  v-model="registerForm.fecha_nacimiento" 
                  required 
                  @click="$event.target.showPicker()"
                />
              </div>
              <div class="input-group">
                <label>Genero</label>
                <select v-model="registerForm.genero" required>
                  <option value="">Seleccione...</option>
                  <option value="M">Masculino</option>
                  <option value="F">Femenino</option>
                </select>
              </div>
            </div>
            <div class="input-group">
              <label>Pais de Origen</label>
              <select v-model="registerForm.id_pais_nacionalidad">
                <option :value="null">Seleccione...</option>
                <option v-for="c in countries" :key="c.id_pais" :value="c.id_pais">{{ c.nombre }}</option>
              </select>
            </div>
            <div class="input-group">
              <label>Correo Electronico</label>
              <input type="email" v-model="registerForm.correo" required placeholder="ejemplo@correo.com"/>
            </div>
          </div>
        </div>

        <div class="input-group">
          <label>Contraseña</label>
          <input type="password" v-model="registerForm.password" required placeholder="Minimo 8 caracteres"/>
        </div>

        <button type="submit" class="submit-btn" :disabled="authStore.isLoading">
          <span v-if="authStore.isLoading">Creando cuenta...</span>
          <span v-else>Registrarse Ahora</span>
        </button>
      </form>

      <div class="back-action">
        <button @click="router.push('/')" class="back-btn">
          ← Volver al inicio
        </button>
      </div>

    </div>
  </div>
</template>

<style scoped>
.auth-container {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 100vh;
  background: linear-gradient(135deg, #f8fafc 0%, #e2e8f0 100%);
  padding: 2rem;
}

.auth-card {
  width: 100%;
  max-width: 450px;
  background: white;
  border-radius: 24px;
  padding: 2.5rem;
  box-shadow: 0 25px 50px -12px rgba(0, 0, 0, 0.1);
  transition: all 0.4s cubic-bezier(0.4, 0, 0.2, 1);
}

.wide-card {
  max-width: 800px;
}

.form-grid {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 2rem;
  margin-bottom: 1rem;
}

@media (max-width: 768px) {
  .form-grid {
    grid-template-columns: 1fr;
  }
}

.tabs {
  display: flex;
  background: #f1f5f9;
  border-radius: 12px;
  padding: 4px;
  margin-bottom: 2rem;
}

.tab-btn {
  flex: 1;
  padding: 10px;
  border: none;
  background: transparent;
  font-weight: 600;
  color: #64748b;
  border-radius: 8px;
  cursor: pointer;
}

.tab-btn.active {
  background: white;
  color: #2563eb;
  box-shadow: 0 2px 4px rgba(0,0,0,0.05);
}

.form-title {
  font-size: 1.8rem;
  font-weight: 700;
  color: #1e293b;
  margin-bottom: 0.5rem;
}

.form-subtitle {
  color: #64748b;
  margin-bottom: 2rem;
}

.form-row {
  display: flex;
  gap: 1rem;
}

.input-group {
  margin-bottom: 1.2rem;
  display: flex;
  flex-direction: column;
  gap: 0.4rem;
  text-align: left;
}

.input-group label {
  font-weight: 600;
  font-size: 0.85rem;
  color: #475569;
}

.input-group input,
.input-group select {
  padding: 12px;
  border-radius: 10px;
  border: 2px solid #e2e8f0;
  font-size: 1rem;
}

.submit-btn {
  width: 100%;
  padding: 14px;
  background: #2563eb;
  color: white;
  border: none;
  border-radius: 12px;
  font-weight: 700;
  font-size: 1.1rem;
  cursor: pointer;
  box-shadow: 0 4px 6px -1px rgba(37, 99, 235, 0.2);
}

.back-action {
  margin-top: 1.5rem;
  border-top: 1px solid #e2e8f0;
  padding-top: 1.5rem;
}

.back-btn {
  background: transparent;
  border: none;
  color: #64748b;
  font-weight: 600;
  cursor: pointer;
}

.fade-in {
  animation: fadeIn 0.4s ease-out;
}

@keyframes fadeIn {
  from { opacity: 0; transform: translateY(10px); }
  to { opacity: 1; transform: translateY(0); }
}
</style>
