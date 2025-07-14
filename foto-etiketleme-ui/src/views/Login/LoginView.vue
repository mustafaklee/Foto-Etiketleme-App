<template>
  <div class="login-container">
    <h2>Giriş Yap</h2>

    <form @submit.prevent="onSubmit">
      <div class="form-group">
        <label for="username">E-posta</label>
        <input
          id="username"
          v-model="form.username"
          type="email"
          class="form-control"
          placeholder="E-posta adresi"
          required
        />
        <span v-if="errors.username" class="text-danger">{{ errors.username }}</span>
      </div>

      <div class="form-group">
        <label for="password">Şifre</label>
        <input
          id="password"
          v-model="form.password"
          type="password"
          class="form-control"
          placeholder="Şifre"
          required
        />
        <span v-if="errors.password" class="text-danger">{{ errors.password }}</span>
      </div>

      <br />
      <button type="submit" class="btn btn-primary" :disabled="loading">
        <span v-if="loading">Giriş yapılıyor...</span>
        <span v-else>Giriş</span>
      </button>
    </form>

    <div v-if="message" class="alert alert-info mt-3">
      {{ message }}
    </div>
  </div>
</template>

<script setup>
import { reactive, ref } from 'vue'
import { useRouter } from 'vue-router'
import { jwtDecode } from 'jwt-decode'
import { loginUser } from '@/js/api.js'

const router = useRouter()

const form = reactive({
  username: '',
  password: ''
})

const errors = reactive({
  username: null,
  password: null
})

const loading = ref(false)
const message = ref(null)

function validate () {
  errors.username = form.username ? null : 'E-posta zorunlu'
  errors.password = form.password ? null : 'Şifre zorunlu'
  return !errors.username && !errors.password
}

async function onSubmit () {
  if (!validate()) return

  loading.value = true
  message.value = null

  try {
    const res = await loginUser({
      Username: form.username,
      Password: form.password
    })

    const token = res.jwtToken || res.token
    if (!token) {
      message.value = 'Sunucudan token alınamadı.'
      return
    }

    // 🔓 Token’ı localStorage’a yaz
    localStorage.setItem('jwtToken', token)

    // 🔍 Token’ı decode ederek rollerini öğren
    const decoded = jwtDecode(token)
    const roles = decoded[
      'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'
    ] || decoded.role || decoded.roles || []
    
    const hasRole = (r) => (Array.isArray(roles) ? roles.includes(r) : roles === r)

    // 🔀 Rol bazlı yönlendirme
    if (hasRole('Admin')) {
      router.push({ name: 'AdminHome' })
    } else {
      router.push({ name: 'Home' })
    }
  } catch (err) {
    message.value = 'Geçersiz kullanıcı adı veya şifre.'
  } finally {
    loading.value = false
  }
}
</script>

<style src="@/assets/login.css"></style>
