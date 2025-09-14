<template>
  <div>
    <!-- Cabeçalho com título e botão -->
    <v-row class="mb-4" align="center">
      <v-col>
        <h2 class="text-h5">Formas de Pagamento</h2>
      </v-col>
      <v-col class="text-right">
        <v-btn color="primary" @click="abrirNovaForma">
          <v-icon left>mdi-plus</v-icon> Nova Forma
        </v-btn>
      </v-col>
    </v-row>

    <!-- Tabela de Formas de Pagamento -->
    <v-data-table
      :headers="headersFormas"
      :items="formasPagamento"
      item-value="idFormaPagamento"
      class="elevation-1"
      dense
      hide-default-footer
    >
      <template #item.nome="{ item }">{{ item.nmFormaPagamento || '—' }}</template>
      <template #item.actions="{ item }">
        <v-btn icon color="green" @click="editarForma(item)">
          <v-icon>mdi-pencil</v-icon>
        </v-btn>
        <v-btn icon color="red" @click="excluirForma(item.idFormaPagamento)">
          <v-icon>mdi-delete</v-icon>
        </v-btn>
      </template>
    </v-data-table>

    <!-- Modal para Cadastro / Edição -->
    <v-dialog v-model="dialogFormaEditar" max-width="400">
      <v-card>
        <v-card-title class="text-h6">
          {{ formaEditando ? 'Editar Forma de Pagamento' : 'Nova Forma de Pagamento' }}
        </v-card-title>

        <v-card-text>
          <v-text-field
            v-model="formaForm.nmFormaPagamento"
            label="Nome da Forma de Pagamento"
            outlined
            dense
          />
        </v-card-text>

        <v-card-actions>
          <v-spacer />
          <v-btn text @click="dialogFormaEditar = false">Cancelar</v-btn>
          <v-btn color="primary" @click="salvarForma">Salvar</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import api from '@/services/api'

const props = defineProps({
  formasPagamento: Array
})

const emit = defineEmits(['recarregar-formas'])

const dialogFormaEditar = ref(false)
const formaEditando = ref(null)
const formaForm = ref({ idFormaPagamento: null, nmFormaPagamento: '' })

const headersFormas = [
  { title: 'Nome', value: 'nome' },
  { title: 'Ações', value: 'actions', sortable: false }
]

// Funções para criar, editar e salvar
function abrirNovaForma() {
  formaEditando.value = null
  formaForm.value = { idFormaPagamento: null, nmFormaPagamento: '' }
  dialogFormaEditar.value = true
}

function editarForma(forma) {
  formaEditando.value = forma
  formaForm.value = { ...forma }
  dialogFormaEditar.value = true
}

async function salvarForma() {
  if (formaEditando.value) {
    await api.put('/api/formapagamento/editar', formaForm.value)
  } else {
    await api.post('/api/formapagamento/cadastrar', formaForm.value)
  }
  dialogFormaEditar.value = false
  emit('recarregar-formas')
}

async function excluirForma(id) {
  await api.delete(`/api/formapagamento/${id}`)
  emit('recarregar-formas')
}
</script>

<style scoped>
.text-right {
  text-align: right;
}
.mb-4 {
  margin-bottom: 1.5rem;
}
</style>
