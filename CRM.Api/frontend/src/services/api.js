// src/services/api.js
import axios from 'axios'
import router from '@/router'
import { useAuthStore } from '@/stores/auth'
import { forceLogout } from '@/helpers/forceLogout'

const api = axios.create({
  baseURL: '/',
  timeout: 20000
})

api.interceptors.request.use((config) => {
  const auth = useAuthStore()
  if (auth.token) {
    config.headers.Authorization = `Bearer ${auth.token}`
  }
  return config
})

api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      import('@/stores/auth').then(({ useAuthStore }) => {
        const auth = useAuthStore()
        auth.clearSession()
        localStorage.clear()
        sessionStorage.clear()
        forceLogout()

        if (router.currentRoute.value.name !== 'Login') {
          router.push({ name: 'Login' })
        }
      })
    }
    return Promise.reject(error)
  }
)


export default api
