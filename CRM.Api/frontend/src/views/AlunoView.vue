<template>
  <v-container>
    <v-row class="mb-4">
      <v-col>
        <h2>Controle de Alunos</h2>
      </v-col>
      <v-col class="text-right">
        <v-btn color="primary" @click="abrirDialogNovo">
          <v-icon start>mdi-plus</v-icon> Novo Aluno
        </v-btn>
      </v-col>
    </v-row>

    <!-- Tabela -->
    <v-data-table
      :headers="headers"
      :items="alunos"
      :loading="loading"
      item-value="idAluno"
      class="elevation-1"
    >
      <template #item.acoes="{ item }">
        <v-btn size="small" icon color="blue" class="me-2" @click="editarAluno(item)">
          <v-icon>mdi-pencil</v-icon>
        </v-btn>
      </template>
    </v-data-table>

    <!-- Dialog Cadastro/Edição -->
    <v-dialog v-model="dialog" max-width="500">
      <v-card>
        <v-card-title>{{ alunoEditando ? 'Editar Aluno' : 'Novo Aluno' }}</v-card-title>
        <v-card-text>
          <v-text-field label="Nome do Aluno" v-model="form.nmAluno" />
          <v-text-field label="Email" v-model="form.email" />
          <v-text-field label="Telefone" v-model.number="form.telefone" type="number"/>
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn text @click="dialog = false">Cancelar</v-btn>
          <v-btn color="primary" @click="salvarAluno">Salvar</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-container>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import api from '@/services/api' // <-- usando api.js com interceptores
import { useAuthStore } from '@/stores/auth'

const auth = useAuthStore()

const alunos = ref([])
const loading = ref(false)
const dialog = ref(false)
const alunoEditando = ref(null)

const form = ref({
  idAluno: null,
  nmAluno: '',
  email: '',
  telefone: ''
})

const headers = [
  { title: 'Nome', value: 'nmAluno' },
  { title: 'Email', value: 'email' },
  { title: 'Telefone', value: 'telefone' },
  { title: 'Ações', value: 'acoes', sortable: false }
]

async function carregarAlunos() {
  loading.value = true
  try {
    const { data } = await api.get('/api/Aluno/listar')
    alunos.value = data.dados || []
  } catch (err) {
    console.error(err)
  } finally {
    loading.value = false
  }
}

function abrirDialogNovo() {
  alunoEditando.value = null
  form.value = { nmAluno: '', email: '', telefone: '' }
  dialog.value = true
}

function editarAluno(aluno) {
  alunoEditando.value = aluno
  form.value = { ...aluno }
  dialog.value = true
}

async function salvarAluno() {
  try {
    let payload = { ...form.value }

    if (alunoEditando.value) {
      await api.put('/api/Aluno/editar', payload)
    } else {
      delete payload.idAluno
      await api.post('/api/Aluno/cadastrar', payload)
    }

    dialog.value = false
    carregarAlunos()
  } catch (err) {
    console.error(err)
  }
}

onMounted(() => {
  carregarAlunos()
})
</script>
