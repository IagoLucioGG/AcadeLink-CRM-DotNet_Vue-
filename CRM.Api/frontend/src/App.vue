<template>
  <router-view />
  <v-alert v-if="erro" type="error" dense outlined fixed top>
    {{ erro }}
  </v-alert>
</template>

<script setup>
import { watch } from 'vue'
import { useRoute } from 'vue-router'
import { useErrorStore } from '@/stores/error'

const { erro } = useErrorStore()
const route = useRoute()

// Nome do sistema padrão (para login ou telas sem empresa)
const nomeSistema = 'AcadeLinkCRM'

// Função para atualizar título e favicon
function atualizarTituloEFavicon() {
  let empresa = null
  const empresaStorage = localStorage.getItem('empresa')
  if (empresaStorage) {
    try {
      empresa = JSON.parse(empresaStorage)
    } catch (e) {
      console.warn('Erro ao parsear empresa do localStorage', e)
    }
  }

  const nomeRota = route.meta.title || route.name || ''

  // Atualiza o título
  if (route.name === 'Login') {
    document.title = nomeSistema
  } else if (empresa && empresa.nmEmpresa) {
    document.title = `${empresa.nmEmpresa} - ${nomeRota}`
  } else {
    document.title = `${nomeSistema} - ${nomeRota}`
  }

  // Atualiza o favicon
  let favicon = document.getElementById('favicon')
  if (!favicon) {
    favicon = document.createElement('link')
    favicon.id = 'favicon'
    favicon.rel = 'icon'
    document.head.appendChild(favicon)
  }
  // Coloque aqui o caminho do seu SVG ou ICO do ícone mdi-link-variant
  favicon.href = '/favicon.svg'
}

// Atualiza quando a rota mudar
watch(() => route.fullPath, atualizarTituloEFavicon, { immediate: true })
</script>
