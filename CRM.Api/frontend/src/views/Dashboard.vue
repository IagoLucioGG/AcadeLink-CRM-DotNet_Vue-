<template>
  <div>
    <h1 class="text-h5 mb-4">Dashboard</h1>
    <v-alert type="info" variant="tonal">
      Bem-vindo, {{ auth.user?.nmAgente }}! <br>
      Universidade: {{ auth.empresa?.nmEmpresa || 'Não informado' }}
    </v-alert>
  </div>
</template>

<script setup>
import { useAuthStore } from '@/stores/auth'

const auth = useAuthStore()
</script>
