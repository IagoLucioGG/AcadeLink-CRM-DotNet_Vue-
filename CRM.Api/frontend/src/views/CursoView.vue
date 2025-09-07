<template>
  <v-container>
    <v-row class="mb-4">
      <v-col>
        <h2>Gerenciar Cursos</h2>
      </v-col>
      <v-col class="text-right">
        <v-btn color="primary" @click="abrirDialogNovo">
          <v-icon start>mdi-plus</v-icon> Novo Curso
        </v-btn>
      </v-col>
    </v-row>

    <!-- Tabela -->
    <v-data-table
      :headers="headers"
      :items="cursos"
      :loading="loading"
      item-value="idCurso"
      class="elevation-1"
    >
      <template #item.acoes="{ item }">
        <v-btn size="small" icon color="blue" class="me-2" @click="editarCurso(item)">
          <v-icon>mdi-pencil</v-icon>
        </v-btn>
      </template>
    </v-data-table>

    <!-- Dialog Cadastro/Edição -->
    <v-dialog v-model="dialog" max-width="500">
      <v-card>
        <v-card-title>{{ cursoEditando ? 'Editar Curso' : 'Novo Curso' }}</v-card-title>
        <v-card-text>
          <v-text-field
            label="Nome do Curso"
            v-model="form.nmCurso"
            :rules="[v => !!v || 'Informe o nome do curso']"
          />
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn text @click="dialog = false">Cancelar</v-btn>
          <v-btn color="primary" @click="salvarCurso">Salvar</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-container>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import api from '@/services/api' // usando interceptores

const cursos = ref([])
const loading = ref(false)
const dialog = ref(false)
const cursoEditando = ref(null)

const form = ref({
  idCurso: null,
  nmCurso: ''
})

const headers = [
  { title: 'Nome do Curso', value: 'nmCurso' },
  { title: 'Ações', value: 'acoes', sortable: false }
]

async function carregarCursos() {
  loading.value = true
  try {
    const { data } = await api.get('/api/Curso/listar')
    cursos.value = data.dados || []
  } catch (err) {
    console.error(err)
  } finally {
    loading.value = false
  }
}

function abrirDialogNovo() {
  cursoEditando.value = null
  form.value = { idCurso: null, nmCurso: '' }
  dialog.value = true
}

function editarCurso(curso) {
  cursoEditando.value = curso
  form.value = { ...curso }
  dialog.value = true
}

async function salvarCurso() {
  try {
    if (cursoEditando.value) {
      await api.put('/api/Curso/editar', {
        idCurso: form.value.idCurso,
        nmCurso: form.value.nmCurso
      })
    } else {
      await api.post('/api/Curso/cadastrar', {
        nmCurso: form.value.nmCurso
      })
    }
    dialog.value = false
    carregarCursos()
  } catch (err) {
    console.error(err)
  }
}

onMounted(() => {
  carregarCursos()
})
</script>
