<template>
  <v-container>
    <v-row>
      <v-col cols="12">
        <PagamentoTable
          :pagamentos="pagamentos"
          :alunos="alunos"
          :matriculas="matriculas"
          :formasPagamento="formasPagamento"
          @recarregar-dados="carregarDados"
          @abrir-formas="mostrarFormas = true"
        />
      </v-col>
    </v-row>

    <!-- Modal sobre a área de Pagamentos -->
    <v-dialog v-model="mostrarFormas" max-width="800px">
      <v-card>
        <v-toolbar flat>
          <v-toolbar-title>Formas de Pagamento</v-toolbar-title>
          <v-spacer></v-spacer>
          <v-btn icon @click="mostrarFormas = false">
            <v-icon>mdi-close</v-icon>
          </v-btn>
        </v-toolbar>
        <v-card-text>
          <FormaPagamentoTable
            :formasPagamento="formasPagamento"
            @recarregar-formas="carregarFormas"
          />
        </v-card-text>
      </v-card>
    </v-dialog>
  </v-container>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import api from '@/services/api'

import PagamentoTable from './Pagamento/PagamentoTable.vue'
import FormaPagamentoTable from './Pagamento/FormaPagamentoTable.vue'

const pagamentos = ref([])
const alunos = ref([])
const matriculas = ref([])
const formasPagamento = ref([])
const mostrarFormas = ref(false)  // controla exibição do modal

async function carregarDados() {
  const resPag = await api.get('/api/pagamento/listar')
  pagamentos.value = resPag.data.dados

  const resAlunos = await api.get('/api/aluno/listar')
  alunos.value = resAlunos.data.dados

  const resMatriculas = await api.get('/api/CursoAluno/listar')
  matriculas.value = resMatriculas.data.dados

  await carregarFormas()
}

async function carregarFormas() {
  const res = await api.get('/api/formapagamento/listar')
  formasPagamento.value = res.data.dados.map(f => ({ ...f, nmFormaPagamento: f.nmFormaPagamento || '—' }))
}

onMounted(carregarDados)
</script>
