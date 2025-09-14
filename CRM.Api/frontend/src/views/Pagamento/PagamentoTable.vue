<template>
    <div>
        <v-row class="mb-4" align="center">
            <v-col>
                <h2 class="text-h5">Pagamentos</h2>
            </v-col>
            <v-col class="text-right">
                <v-btn color="primary" @click="abrirDialogNovo">
                    <v-icon left>mdi-plus</v-icon> Novo Pagamento
                </v-btn>
                <v-btn color="secondary" class="ml-2" @click="$emit('abrir-formas')">
                    <v-icon left>mdi-cash</v-icon> Formas de Pagamento
                </v-btn>
            </v-col>
        </v-row>

        <!-- Tabela de Pagamentos -->
        <v-data-table :headers="headers" :items="pagamentos" item-value="idPagamento" class="elevation-1" dense>
            <template #item.aluno="{ item }">{{ item.aluno.nmAluno }}</template>
            <template #item.matricula="{ item }">{{ item.matricula.nrMatricula }}</template>
            <template #item.formaPagamento="{ item }">{{ item.formaPagamento?.nome || '—' }}</template>
            <template #item.dataReferentePagamento="{ item }">{{ formatarData(item.dataReferentePagamento) }}</template>
            <template #item.actions="{ item }">
                <v-btn icon color="blue" @click="abrirDetalhes(item)">
                    <v-icon>mdi-eye</v-icon>
                </v-btn>
                <v-btn icon color="green" @click="editar(item)">
                    <v-icon>mdi-pencil</v-icon>
                </v-btn>
                <v-btn icon color="red" @click="excluir(item.idPagamento)">
                    <v-icon>mdi-delete</v-icon>
                </v-btn>
            </template>
        </v-data-table>

        <!-- Modal Cadastro / Edição Pagamento -->
        <v-dialog v-model="dialog" max-width="500">
            <v-card>
                <v-card-title>
                    {{ pagamentoEditando ? 'Editar Pagamento' : 'Novo Pagamento' }}
                </v-card-title>
                <v-card-text>
                    <v-row>
                        <v-col cols="12" sm="6">
                            <v-autocomplete label="Aluno" v-model="form.idAluno" :items="alunos" item-title="nmAluno"
                                item-value="idAluno" outlined dense @change="onAlunoChange" />
                        </v-col>
                        <v-col cols="12" sm="6">
                            <v-autocomplete label="Matrícula" v-model="form.idMatricula" :items="matriculasFiltradas"
                                item-title="nrMatricula" item-value="idMatricula" outlined dense
                                @change="onMatriculaChange" />
                        </v-col>
                    </v-row>

                    <v-row>
                        <v-col cols="12" sm="6">
                            <v-select label="Forma de Pagamento" :items="formasPagamento" item-title="nmFormaPagamento"
                                item-value="idFormaPagamento" v-model="form.idFormaPagamento" outlined dense />
                        </v-col>
                        <v-col cols="12" sm="6">
                            <v-text-field label="Data Referente" type="date" v-model="form.dataReferentePagamento"
                                outlined dense />
                        </v-col>
                    </v-row>

                    <v-row>
                        <v-col cols="12" sm="6">
                            <v-text-field label="Valor Devido" type="number" step="0.01"
                                v-model.number="form.valorDevido" outlined dense />
                        </v-col>
                        <v-col cols="12" sm="6">
                            <v-text-field label="Valor Pago" type="number" step="0.01" v-model.number="form.valorPago"
                                outlined dense />
                        </v-col>
                    </v-row>
                </v-card-text>
                <v-card-actions>
                    <v-spacer />
                    <v-btn text @click="dialog = false">Cancelar</v-btn>
                    <v-btn color="primary" @click="salvarPagamento">Salvar</v-btn>
                </v-card-actions>
            </v-card>
        </v-dialog>

        <!-- Modal Detalhes do Pagamento -->
        <v-dialog v-model="dialogDetalhes" max-width="500">
            <v-card>
                <v-card-title>Detalhes do Pagamento</v-card-title>
                <v-card-text>
                    <p><strong>Aluno:</strong> {{ detalhes.aluno.nmAluno }}</p>
                    <p><strong>Matrícula:</strong> {{ detalhes.matricula.nrMatricula }}</p>
                    <p><strong>Forma de Pagamento:</strong> {{ detalhes.formaPagamento?.nome || '—' }}</p>
                    <p><strong>Data Inclusão:</strong> {{ formatarData(detalhes.dataInclusao) }}</p>
                    <p><strong>Data Referente:</strong> {{ formatarData(detalhes.dataReferentePagamento) }}</p>
                    <p><strong>Valor Devido:</strong> R${{ detalhes.valorDevido.toFixed(2) }}</p>
                    <p><strong>Valor Pago:</strong> R${{ detalhes.valorPago.toFixed(2) }}</p>
                    <p><strong>Valor Restante:</strong> R${{ detalhes.valorRestante.toFixed(2) }}</p>
                </v-card-text>
                <v-card-actions>
                    <v-spacer />
                    <v-btn text @click="dialogDetalhes = false">Fechar</v-btn>
                </v-card-actions>
            </v-card>
        </v-dialog>
    </div>
</template>

<script setup>
import { ref, computed } from 'vue'
import api from '@/services/api'

const props = defineProps({
    pagamentos: Array,
    alunos: Array,
    matriculas: Array,
    formasPagamento: Array
})

const emit = defineEmits(['recarregar-dados', 'abrir-formas'])

const dialog = ref(false)
const dialogDetalhes = ref(false)
const pagamentoEditando = ref(null)
const detalhes = ref({})

const form = ref({
    idPagamento: null,
    dataReferentePagamento: '',
    idFormaPagamento: null,
    idAluno: null,
    idMatricula: null,
    valorDevido: 0,
    valorPago: 0
})

// Computeds
const matriculasFiltradas = computed(() => {
    if (form.value.idAluno) {
        return props.matriculas
            .filter(m => m.aluno.idAluno === form.value.idAluno)
            .map(m => ({ ...m, descricao: m.nrMatricula }))
    }
    return props.matriculas.map(m => ({ ...m, descricao: m.nrMatricula }))
})

// Dependência entre campos
function onAlunoChange() {
    if (form.value.idAluno && form.value.idMatricula) {
        const matriculaValida = props.matriculas.find(
            m => m.idMatricula === form.value.idMatricula && m.aluno.idAluno === form.value.idAluno
        )
        if (!matriculaValida) form.value.idMatricula = null
    }
}

function onMatriculaChange() {
    if (form.value.idMatricula) {
        const matricula = props.matriculas.find(m => m.idMatricula === form.value.idMatricula)
        if (matricula) form.value.idAluno = matricula.aluno.idAluno
    }
}

// CRUD
function abrirDialogNovo() {
    pagamentoEditando.value = null
    form.value = {
        idPagamento: null,
        dataReferentePagamento: new Date().toISOString().substr(0, 10),
        idFormaPagamento: null,
        idAluno: null,
        idMatricula: null,
        valorDevido: 0,
        valorPago: 0
    }
    dialog.value = true
}

function editar(pagamento) {
    pagamentoEditando.value = pagamento

    // Achar matrícula correspondente para descrição
    const matricula = props.matriculas.find(m => m.idMatricula === pagamento.matricula?.idMatricula)

    form.value = {
        idPagamento: pagamento.idPagamento,
        idAluno: pagamento.aluno?.idAluno || null,
        idMatricula: matricula ? matricula.idMatricula : null,
        idFormaPagamento: pagamento.formaPagamento?.idFormaPagamento || null,
        dataReferentePagamento: pagamento.dataReferentePagamento
            ? new Date(pagamento.dataReferentePagamento).toISOString().substr(0, 10)
            : '',
        valorDevido: pagamento.valorDevido || 0,
        valorPago: pagamento.valorPago || 0
    }

    dialog.value = true
}

async function salvarPagamento() {
    if (pagamentoEditando.value) {
        await api.put('/api/pagamento/editar', form.value)
    } else {
        await api.post('/api/pagamento/cadastrar', form.value)
    }
    dialog.value = false
    emit('recarregar-dados')
}

async function excluir(id) {
    await api.delete(`/api/pagamento/${id}`)
    emit('recarregar-dados')
}

function abrirDetalhes(pagamento) {
    detalhes.value = pagamento
    dialogDetalhes.value = true
}

// Formatar datas
function formatarData(data) {
    if (!data) return '—'
    return new Date(data).toLocaleDateString('pt-BR')
}

// Headers
const headers = [
    { title: 'Aluno', value: 'aluno' },
    { title: 'Matrícula', value: 'matricula' },
    { title: 'Forma de Pagamento', value: 'formaPagamento' },
    { title: 'Data Referente', value: 'dataReferentePagamento' },
    { title: 'Ações', value: 'actions', sortable: false }
]
</script>
