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
      <template #item.cpf="{ item }">
        {{ formatarCPF(item.cpf) }}
      </template>

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
          <v-alert v-if="erro" type="error" dense outlined class="mb-2">
            {{ erro }}
          </v-alert>

          <v-text-field label="Nome do Aluno" v-model="form.nmAluno" />

          <v-text-field
            label="CPF"
            v-model="form.cpf"
            maxlength="14"
            @input="mascararCPF"
          />

          <v-text-field label="Email" v-model="form.email" />

          <v-text-field
            label="Telefone"
            v-model="form.telefone"
            type="number"
          />
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
import api from '@/services/api'

const alunos = ref([])
const loading = ref(false)
const dialog = ref(false)
const alunoEditando = ref(null)
const erro = ref('')

const form = ref({
  idAluno: null,
  nmAluno: '',
  cpf: '',
  email: '',
  telefone: ''
})

const headers = [
  { title: 'Nome', value: 'nmAluno' },
  { title: 'CPF', value: 'cpf' },
  { title: 'Email', value: 'email' },
  { title: 'Telefone', value: 'telefone' },
  { title: 'Ações', value: 'acoes', sortable: false }
]

// 🔹 Buscar alunos
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

// 🔹 Novo aluno
function abrirDialogNovo() {
  alunoEditando.value = null
  form.value = { nmAluno: '', cpf: '', email: '', telefone: '' }
  erro.value = ''
  dialog.value = true
}

// 🔹 Editar aluno
function editarAluno(aluno) {
  alunoEditando.value = aluno
  form.value = { ...aluno }
  erro.value = ''
  dialog.value = true
}

// 🔹 Salvar aluno
async function salvarAluno() {
  try {
    erro.value = ''
    let payload = { ...form.value }
    // remover máscara antes de enviar
    payload.cpf = payload.cpf.replace(/\D/g, '')

    if (alunoEditando.value) {
      await api.put('/api/Aluno/editar', payload)
    } else {
      delete payload.idAluno
      await api.post('/api/Aluno/cadastrar', payload)
    }

    dialog.value = false
    carregarAlunos()
  } catch (err) {
    if (err.response && err.response.data && err.response.data.mensagem) {
      erro.value = err.response.data.mensagem
    } else {
      erro.value = 'Erro ao salvar o aluno. Tente novamente.'
    }
    console.error(err)
  }
}

// 🔹 Função para formatar CPF na tabela
function formatarCPF(cpf) {
  if (!cpf) return ''
  const v = cpf.replace(/\D/g, '').padStart(11, '0')
  return v.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, '$1.$2.$3-$4')
}

// 🔹 Máscara no input
function mascararCPF() {
  let v = form.value.cpf.replace(/\D/g, '')
  v = v.substring(0, 11) // máximo 11 dígitos
  if (v.length > 9) {
    form.value.cpf = v.replace(/(\d{3})(\d{3})(\d{3})(\d{0,2})/, '$1.$2.$3-$4')
  } else if (v.length > 6) {
    form.value.cpf = v.replace(/(\d{3})(\d{3})(\d{0,3})/, '$1.$2.$3')
  } else if (v.length > 3) {
    form.value.cpf = v.replace(/(\d{3})(\d{0,3})/, '$1.$2')
  } else {
    form.value.cpf = v
  }
}

onMounted(() => {
  carregarAlunos()
})
</script>
