<template>
  <v-snackbar
    v-model="exibir"
    color="red"
    top
    right
    :timeout="5000"
  >
    {{ erro }}
    <template #action>
      <v-btn icon @click="limpar">
        <v-icon>mdi-close</v-icon>
      </v-btn>
    </template>
  </v-snackbar>
</template>

<script setup>
import { computed, watch } from 'vue'
import { useErrorStore } from '@/stores/error'

const errorStore = useErrorStore()
const erro = computed(() => errorStore.erro)
const exibir = computed({
  get: () => !!errorStore.erro,
  set: (val) => {
    if (!val) errorStore.limparErro()
  }
})

function limpar() {
  errorStore.limparErro()
}

// Auto fecha após 5s
watch(erro, (v) => {
  if (v) setTimeout(() => errorStore.limparErro(), 5000)
})
</script>
