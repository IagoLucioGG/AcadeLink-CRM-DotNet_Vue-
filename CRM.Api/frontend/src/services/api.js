// src/services/api.js
import axios from 'axios'
import router from '@/router'
import { useAuthStore } from '@/stores/auth'
import { forceLogout } from '@/helpers/forceLogout'
import { useErrorStore } from '@/stores/error'// <-- store global de erros

const api = axios.create({
  baseURL: '/',
  timeout: 20000
})

// Interceptor de requisição: adiciona token
api.interceptors.request.use((config) => {
  const auth = useAuthStore()
  if (auth.token) {
    config.headers.Authorization = `Bearer ${auth.token}`
  }
  return config
})

// Interceptor de resposta: tratamento de erros
api.interceptors.response.use(
  (response) => response,
  (error) => {
    const errorStore = useErrorStore()

    // 401 - não autorizado
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

    // Outras mensagens de erro da API
    if (error.response?.data) {
      const data = error.response.data
      if (data.mensagem) {
        errorStore.setErro(data.mensagem)
      } else if (data.errors) {
        // caso seja validação padrão .NET
        const msgs = Object.values(data.errors).flat().join(', ')
        errorStore.setErro(msgs)
      } else {
        errorStore.setErro('Ocorreu um erro inesperado.')
      }
    } else {
      errorStore.setErro('Erro de conexão com o servidor.')
    }

    return Promise.reject(error)
  }
)

export default api
