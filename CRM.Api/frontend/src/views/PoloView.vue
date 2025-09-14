<template>
  <v-container fluid class="pa-4">
    <v-row class="mb-4">
      <v-col>
        <h2>Gestão de Polos</h2>
      </v-col>
      <v-col class="text-right">
        <v-btn color="primary" @click="abrirDialogNovo">
          <v-icon start>mdi-plus</v-icon> Novo Polo
        </v-btn>
      </v-col>
    </v-row>

    <!-- Tabela -->
    <v-data-table
      :headers="headers"
      :items="polos"
      :loading="loading"
      item-value="idPolo"
      class="elevation-1"
    >
      <template #item.acoes="{ item }">
        <v-btn size="small" icon color="blue" @click="editarPolo(item)">
          <v-icon>mdi-pencil</v-icon>
        </v-btn>
        <v-btn size="small" icon color="red" @click="excluirPolo(item)">
          <v-icon>mdi-delete</v-icon>
        </v-btn>
      </template>
    </v-data-table>

    <!-- Modal de cadastro/edição -->
    <v-dialog v-model="dialog" max-width="500">
      <v-card>
        <v-card-title>{{ poloEditando ? 'Editar Polo' : 'Novo Polo' }}</v-card-title>
        <v-card-text>
          <v-text-field
            label="Nome do Polo"
            v-model="form.nmPolo"
            :rules="[v => !!v || 'Informe o nome do polo']"
            clearable
          />
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn text @click="dialog = false">Cancelar</v-btn>
          <v-btn color="primary" @click="salvarPolo">Salvar</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-container>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import api from '@/services/api'

const polos = ref([])
const loading = ref(false)
const dialog = ref(false)
const poloEditando = ref(null)

const form = ref({
  idPolo: null,
  nmPolo: ''
})

const headers = [
  { title: 'ID', value: 'idPolo' },
  { title: 'Nome do Polo', value: 'nmPolo' },
  { title: 'Ações', value: 'acoes', sortable: false }
]

// 🔹 Listar polos
async function carregarPolos() {
  loading.value = true
  try {
    const { data } = await api.get('/api/Polo/listar')
    polos.value = data.dados || []
  } finally {
    loading.value = false
  }
}

// 🔹 Abrir modal de cadastro
function abrirDialogNovo() {
  poloEditando.value = null
  form.value = { idPolo: null, nmPolo: '' }
  dialog.value = true
}

// 🔹 Abrir modal de edição
function editarPolo(polo) {
  poloEditando.value = polo
  form.value = { idPolo: polo.idPolo, nmPolo: polo.nmPolo }
  dialog.value = true
}

// 🔹 Salvar polo (POST/PUT)
async function salvarPolo() {
  try {
    if (poloEditando.value) {
      await api.put('/api/Polo', form.value)
    } else {
      await api.post('/api/Polo', form.value)
    }
    dialog.value = false
    carregarPolos()
  } catch (err) {
    console.error(err)
  }
}

// 🔹 Excluir polo
async function excluirPolo(polo) {
  if (confirm(`Deseja realmente excluir o polo "${polo.nmPolo}"?`)) {
    try {
      await api.delete(`/api/Polo/${polo.idPolo}`)
      carregarPolos()
    } catch (err) {
      console.error(err)
    }
  }
}

onMounted(() => {
  carregarPolos()
})
</script>
