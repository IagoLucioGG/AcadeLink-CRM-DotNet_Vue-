<template>
  <v-container>
    <v-row class="mb-4">
      <v-col>
        <h2>Controle de Usuários</h2>
      </v-col>
      <v-col class="text-right">
        <v-btn color="primary" @click="abrirDialogNovo">
          <v-icon start>mdi-plus</v-icon> Novo Usuário
        </v-btn>
      </v-col>
    </v-row>
    
    <!-- Tabela -->
    <v-data-table
      :headers="headers"
      :items="usuarios"
      :loading="loading"
      item-value="idUsuario"
      class="elevation-1"
    >
      <template #item.ativo="{ item }">
        <v-chip :color="item.ativo ? 'green' : 'red'" dark>
          {{ item.ativo ? 'Ativo' : 'Inativo' }}
        </v-chip>
      </template>

      <template #item.acoes="{ item }">
        <v-btn
          size="small"
          icon
          color="blue"
          class="me-2"
          @click="editarUsuario(item)"
        >
          <v-icon>mdi-pencil</v-icon>
        </v-btn>

        <v-btn
          size="small"
          icon
          :color="item.ativo ? 'red' : 'green'"
          @click="alterarStatus(item)"
        >
          <v-icon>{{ item.ativo ? 'mdi-account-off' : 'mdi-account-check' }}</v-icon>
        </v-btn>
      </template>
    </v-data-table>

    <!-- Dialog Cadastro/Edição -->
    <v-dialog v-model="dialog" max-width="500">
      <v-card>
        <v-card-title>{{ usuarioEditando ? 'Editar Usuário' : 'Novo Usuário' }}</v-card-title>
        <v-card-text>
          <v-text-field label="Nome do Agente" v-model="form.nmAgente" />
          <v-text-field label="Login" v-model="form.nmLogin" />
          <v-text-field label="Senha" v-model="form.senha" type="password" />
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn text @click="dialog = false">Cancelar</v-btn>
          <v-btn color="primary" @click="salvarUsuario">Salvar</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-container>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import api from '@/services/api' // usando interceptores

const usuarios = ref([])
const loading = ref(false)
const dialog = ref(false)
const usuarioEditando = ref(null)

const form = ref({
  idUsuario: null,
  nmAgente: '',
  nmLogin: '',
  senha: ''
})

const headers = [
  { title: 'ID', value: 'idUsuario' },
  { title: 'Nome', value: 'nmAgente' },
  { title: 'Login', value: 'nmLogin' },
  { title: 'Status', value: 'ativo' },
  { title: 'Data de Criação', value: 'dataCriacao' },
  { title: 'Ações', value: 'acoes', sortable: false }
]

async function carregarUsuarios() {
  loading.value = true
  try {
    const { data } = await api.get('/api/Usuario/listar')
    usuarios.value = data.dados || []
  } catch (err) {
    console.error(err)
  } finally {
    loading.value = false
  }
}

function abrirDialogNovo() {
  usuarioEditando.value = null
  form.value = { idUsuario: null, nmAgente: '', nmLogin: '', senha: '' }
  dialog.value = true
}

function editarUsuario(usuario) {
  usuarioEditando.value = usuario
  form.value = { ...usuario, senha: '' }
  dialog.value = true
}

async function salvarUsuario() {
  try {
    if (usuarioEditando.value) {
      await api.put('/api/Usuario/editar', form.value)
    } else {
      await api.post('/api/Usuario/cadastrar', form.value)
    }
    dialog.value = false
    carregarUsuarios()
  } catch (err) {
    console.error(err)
  }
}

async function alterarStatus(usuario) {
  try {
    await api.patch(`/api/Usuario/${usuario.idUsuario}/status?ativo=${!usuario.ativo}`)
    carregarUsuarios()
  } catch (err) {
    console.error(err)
  }
}

onMounted(() => {
  carregarUsuarios()
})
</script>
