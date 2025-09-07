// src/helpers/forceLogout.js
import router from '@/router'

export function forceLogout() {
  try {
    // Limpa todos os dados salvos
    localStorage.clear()
    sessionStorage.clear()

    // Garante que o redirecionamento só aconteça se não estiver no Login
    setTimeout(() => {
      if (router.currentRoute.value.name !== 'Login') {
        router.push({ name: 'Login' })
      }
    }, 0)
  } catch (err) {
    console.error('Erro ao forçar logout', err)
  }
}
