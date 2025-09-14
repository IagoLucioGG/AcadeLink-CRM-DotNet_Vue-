<template>
  <v-container class="pa-0">
    <v-row class="mb-4">
      <v-col>
        <h2>Matriculas</h2>
      </v-col>
      <v-col class="text-right">
        <v-btn color="primary" @click="abrirDialogNovo">
          <v-icon start>mdi-plus</v-icon> Nova Matrícula
        </v-btn>
      </v-col>
    </v-row>

    <!-- Tabela principal -->
    <v-data-table
      :headers="headers"
      :items="matriculas"
      :loading="loading"
      item-value="idCursoAluno"
      class="elevation-1"
    >
      <template #item.aluno="{ item }">{{ item.aluno.nmAluno }}</template>
      <template #item.curso="{ item }">{{ item.curso.nmCurso }}</template>
      <template #item.status="{ item }">
        <v-chip :color="item.dataCancelamento ? 'red' : 'green'" dark>
          {{ item.dataCancelamento ? 'Cancelada' : 'Ativa' }}
        </v-chip>
      </template>
      <template #item.acoes="{ item }">
        <v-btn size="small" icon color="blue" @click="editarMatricula(item)">
          <v-icon>mdi-pencil</v-icon>
        </v-btn>
        <v-btn
          v-if="!item.dataCancelamento"
          size="small"
          icon
          color="red"
          @click="cancelarMatricula(item)"
        >
          <v-icon>mdi-close-circle</v-icon>
        </v-btn>
        <v-btn size="small" icon color="grey" @click="abrirDetalhes(item)">
          <v-icon>mdi-eye</v-icon>
        </v-btn>
      </template>
    </v-data-table>

    <!-- Modal Cadastro / Edição -->
    <v-dialog v-model="dialog" max-width="500">
      <v-card>
        <v-card-title>
          {{ matriculaEditando ? 'Editar Matrícula' : 'Nova Matrícula' }}
        </v-card-title>
        <v-card-text>
          <v-row>
            <v-col cols="12" sm="6">
              <v-autocomplete
                label="Aluno"
                v-model="form.idAluno"
                :items="alunos"
                item-title="nmAluno"
                item-value="idAluno"
                :search-input.sync="searchAluno"
                :loading="loadingAlunos"
                hide-no-data
                hide-details
                @update:search-input="filtrarAlunos"
                :rules="[v => !!v || 'Selecione um aluno']"
              />
            </v-col>
            <v-col cols="12" sm="6">
              <v-autocomplete
                label="Curso"
                v-model="form.idCurso"
                :items="cursos"
                item-title="nmCurso"
                item-value="idCurso"
                :search-input.sync="searchCurso"
                :loading="loadingCursos"
                hide-no-data
                hide-details
                @update:search-input="filtrarCursos"
                :rules="[v => !!v || 'Selecione um curso']"
              />
            </v-col>
          </v-row>

          <v-row>
            <v-col cols="12" sm="4">
              <v-select
                label="Modalidade"
                :items="modalidades"
                item-title="nmModalidade"
                item-value="idModalidade"
                v-model="form.idModalidade"
                :rules="[v => !!v || 'Selecione uma modalidade']"
              />
            </v-col>
            <v-col cols="12" sm="4">
              <v-select
                label="Polo"
                :items="polos"
                item-title="nmPolo"
                item-value="idPolo"
                v-model="form.idPolo"
              />
            </v-col>
            <v-col cols="12" sm="4">
              <v-text-field
                label="Nr. Matrícula"
                type="number"
                v-model.number="form.nrMatricula"
                :rules="[v => v > 0 || 'Informe um número válido']"
              />
            </v-col>
          </v-row>

          <v-row>
            <v-col cols="12" sm="6">
              <v-text-field
                label="Data Matrícula"
                type="date"
                v-model="form.dataMatricula"
              />
            </v-col>
            <v-col cols="12" sm="6" v-if="matriculaEditando">
              <v-text-field
                label="Data Cancelamento"
                type="date"
                v-model="form.dataCancelamento"
              />
            </v-col>
          </v-row>
        </v-card-text>

        <v-card-actions>
          <v-spacer />
          <v-btn text @click="dialog = false">Cancelar</v-btn>
          <v-btn color="primary" @click="salvarMatricula">Salvar</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <!-- Modal Detalhes -->
    <v-dialog v-model="dialogDetalhes" max-width="500">
      <v-card>
        <v-card-title>Detalhes da Matrícula</v-card-title>
        <v-card-text v-if="matriculaDetalhes">
          <p><strong>Aluno:</strong> {{ matriculaDetalhes.aluno.nmAluno }}</p>
          <p><strong>Email:</strong> {{ matriculaDetalhes.aluno.email }}</p>
          <p><strong>Telefone:</strong> {{ matriculaDetalhes.aluno.telefone }}</p>
          <p><strong>Curso:</strong> {{ matriculaDetalhes.curso.nmCurso }}</p>
          <p><strong>Modalidade:</strong> {{ matriculaDetalhes.modalidade.nmModalidade }}</p>
          <p><strong>Polo:</strong> {{ matriculaDetalhes.polo?.nmPolo || '—' }}</p>
          <p><strong>Nr. Matrícula:</strong> {{ matriculaDetalhes.nrMatricula }}</p>
          <p>
            <strong>Data Matrícula:</strong>
            {{ formatarData(matriculaDetalhes.dataMatricula) }}
          </p>
          <p>
            <strong>Data Cancelamento:</strong>
            {{ matriculaDetalhes.dataCancelamento ? formatarData(matriculaDetalhes.dataCancelamento) : '—' }}
          </p>
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn color="primary" text @click="dialogDetalhes = false">Fechar</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-container>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import api from '@/services/api'

const matriculas = ref([])
const alunos = ref([])
const cursos = ref([])
const modalidades = ref([])
const polos = ref([])

const loading = ref(false)
const dialog = ref(false)
const matriculaEditando = ref(null)
const dialogDetalhes = ref(false)
const matriculaDetalhes = ref(null)

const form = ref({
  idCursoAluno: null,
  idAluno: null,
  idCurso: null,
  idModalidade: null,
  idPolo: null,
  nrMatricula: 0,
  dataMatricula: '',
  dataCancelamento: ''
})

const searchAluno = ref('')
const searchCurso = ref('')
const loadingAlunos = ref(false)
const loadingCursos = ref(false)

const headers = [
  { title: 'Aluno', value: 'aluno' },
  { title: 'Curso', value: 'curso' },
  { title: 'Status', value: 'status' },
  { title: 'Ações', value: 'acoes', sortable: false }
]

// 🔹 Listas auxiliares
async function carregarAlunos(filtro = '') {
  loadingAlunos.value = true
  try {
    const { data } = await api.get('/api/Aluno/listar', { params: { filtro } })
    alunos.value = data.dados || []
  } finally {
    loadingAlunos.value = false
  }
}

async function carregarCursos(filtro = '') {
  loadingCursos.value = true
  try {
    const { data } = await api.get('/api/Curso/listar', { params: { filtro } })
    cursos.value = data.dados || []
  } finally {
    loadingCursos.value = false
  }
}

async function carregarModalidades() {
  const { data } = await api.get('/api/Modalidade/listar')
  modalidades.value = data.dados || []
}

async function carregarPolos() {
  const { data } = await api.get('/api/Polo/listar')
  polos.value = data.dados || []
}

// 🔹 Listar matrículas
async function carregarMatriculas() {
  loading.value = true
  try {
    const { data } = await api.get('/api/CursoAluno/listar')
    matriculas.value = data.dados || []
  } finally {
    loading.value = false
  }
}

// abrir cadastro
function abrirDialogNovo() {
  matriculaEditando.value = null
  form.value = {
    idAluno: null,
    idCurso: null,
    idModalidade: null,
    idPolo: null,
    nrMatricula: 0,
    dataMatricula: new Date().toISOString().substr(0, 10),
    dataCancelamento: ''
  }
  dialog.value = true
}

// abrir edição
function editarMatricula(matricula) {
  matriculaEditando.value = matricula
  form.value = {
    idCursoAluno: matricula.idCursoAluno,
    idAluno: matricula.aluno.idAluno,
    idCurso: matricula.curso.idCurso,
    idModalidade: matricula.modalidade.idModalidade,
    idPolo: matricula.polo?.idPolo || null,
    nrMatricula: matricula.nrMatricula,
    dataMatricula: matricula.dataMatricula.substr(0, 10),
    dataCancelamento: matricula.dataCancelamento
      ? matricula.dataCancelamento.substr(0, 10)
      : ''
  }
  dialog.value = true
}

// salvar matrícula
async function salvarMatricula() {
  try {
    if (matriculaEditando.value) {
      // 🔹 Se não informar dataCancelamento → matrícula reativada
      if (!form.value.dataCancelamento) {
        form.value.dataCancelamento = null
      }
      await api.put('/api/CursoAluno/editar', form.value)
    } else {
      await api.post('/api/CursoAluno/matricular', form.value)
    }
    dialog.value = false
    carregarMatriculas()
  } catch (err) {
    console.error(err)
  }
}

// cancelar matrícula
async function cancelarMatricula(matricula) {
  if (confirm('Deseja realmente cancelar esta matrícula?')) {
    await api.patch(`/api/CursoAluno/${matricula.idCursoAluno}/cancelar`)
    carregarMatriculas()
  }
}

// filtros dinâmicos
function filtrarAlunos(valor) {
  carregarAlunos(valor)
}
function filtrarCursos(valor) {
  carregarCursos(valor)
}

// modal detalhes
function abrirDetalhes(matricula) {
  matriculaDetalhes.value = matricula
  dialogDetalhes.value = true
}

// formatação de datas
function formatarData(dataString) {
  if (!dataString) return '—'
  const dt = new Date(dataString)
  return new Intl.DateTimeFormat('pt-BR', {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric'
  }).format(dt)
}

// on mounted
onMounted(() => {
  carregarMatriculas()
  carregarAlunos()
  carregarCursos()
  carregarModalidades()
  carregarPolos()
})
</script>
