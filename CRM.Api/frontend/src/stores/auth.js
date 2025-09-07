// src/stores/auth.js
import { defineStore } from 'pinia'

// helper para decodificar o payload do JWT
function parseJwt(token) {
  try {
    const base64Url = token.split('.')[1]
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/')
    const jsonPayload = decodeURIComponent(
      atob(base64)
        .split('')
        .map(c => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
        .join('')
    )
    return JSON.parse(jsonPayload)
  } catch {
    return null
  }
}

export const useAuthStore = defineStore('auth', {
  state: () => ({
    token: localStorage.getItem('token') || '',
    expMs: Number(localStorage.getItem('token_exp_ms') || 0),
    user: JSON.parse(localStorage.getItem('user') || 'null'),
    empresa: JSON.parse(localStorage.getItem('empresa') || 'null')
  }),
  getters: {
    isAuthenticated: (state) => {
      if (!state.token || !state.expMs) return false
      return Date.now() < state.expMs
    }
  },
  actions: {
    setSession(token, user, empresa) {
      this.token = token
      this.user = user || null
      this.empresa = empresa || null

      const payload = parseJwt(token)
      // usa 'exp' do JWT (segundos desde epoch)
      const expMs = payload?.exp ? payload.exp * 1000 : 0
      this.expMs = expMs

      localStorage.setItem('token', this.token)
      localStorage.setItem('token_exp_ms', String(this.expMs))
      localStorage.setItem('user', JSON.stringify(this.user))
      localStorage.setItem('empresa', JSON.stringify(this.empresa))
    },
    clearSession() {
      this.token = ''
      this.expMs = 0
      this.user = null
      this.empresa = null
      localStorage.removeItem('token')
      localStorage.removeItem('token_exp_ms')
      localStorage.removeItem('user')
      localStorage.removeItem('empresa')
    }
  }
})
