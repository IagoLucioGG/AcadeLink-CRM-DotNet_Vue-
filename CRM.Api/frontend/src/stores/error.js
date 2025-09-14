import { ref } from 'vue'

export const useErrorStore = () => {
  const erro = ref('')

  function setErro(msg) {
    erro.value = msg
    // Limpar após 5 segundos, opcional
    setTimeout(() => { erro.value = '' }, 5000)
  }

  function clearErro() {
    erro.value = ''
  }

  return { erro, setErro, clearErro }
}
