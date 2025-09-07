<template>
    <v-container>
        <v-row class="mb-4">
            <v-col>
                <h2>Gerenciar Modalidades</h2>
            </v-col>
            <v-col class="text-right">
                <v-btn color="primary" @click="abrirDialogNovo">
                    <v-icon start>mdi-plus</v-icon> Nova Modalidade
                </v-btn>
            </v-col>
        </v-row>

        <!-- Tabela -->
        <v-data-table
            :headers="headers"
            :items="modalidades"
            :loading="loading"
            item-value="idModalidade"
            class="elevation-1"
        >
            <template #item.acoes="{ item }">
                <v-btn size="small" icon color="blue" class="me-2" @click="editarModalidade(item)">
                    <v-icon>mdi-pencil</v-icon>
                </v-btn>
            </template>
        </v-data-table>

        <!-- Dialog Cadastro/Edição -->
        <v-dialog v-model="dialog" max-width="500">
            <v-card>
                <v-card-title>{{ modalidadeEditando ? 'Editar Modalidade' : 'Nova Modalidade' }}</v-card-title>
                <v-card-text>
                    <v-text-field
                        label="Nome da Modalidade"
                        v-model="form.nmModalidade"
                        :rules="[v => !!v || 'Informe o nome da modalidade']"
                    />
                </v-card-text>
                <v-card-actions>
                    <v-spacer />
                    <v-btn text @click="dialog = false">Cancelar</v-btn>
                    <v-btn color="primary" @click="salvarModalidade">Salvar</v-btn>
                </v-card-actions>
            </v-card>
        </v-dialog>
    </v-container>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import api from '@/services/api' // <-- usando api.js com interceptores

const modalidades = ref([])
const loading = ref(false)
const dialog = ref(false)
const modalidadeEditando = ref(null)

const form = ref({
    idModalidade: null,
    nmModalidade: ''
})

const headers = [
    { title: 'Nome', value: 'nmModalidade' },
    { title: 'Ações', value: 'acoes', sortable: false }
]

async function carregarModalidades() {
    loading.value = true
    try {
        const { data } = await api.get('/api/Modalidade/listar')
        modalidades.value = data.dados || []
    } catch (err) {
        console.error(err)
    } finally {
        loading.value = false
    }
}

function abrirDialogNovo() {
    modalidadeEditando.value = null
    form.value = { idModalidade: null, nmModalidade: '' }
    dialog.value = true
}

function editarModalidade(modalidade) {
    modalidadeEditando.value = modalidade
    form.value = { ...modalidade }
    dialog.value = true
}

async function salvarModalidade() {
    try {
        if (modalidadeEditando.value) {
            await api.put('/api/Modalidade/editar', form.value)
        } else {
            await api.post('/api/Modalidade/cadastrar', {
                nmModalidade: form.value.nmModalidade
            })
        }
        dialog.value = false
        carregarModalidades()
    } catch (err) {
        console.error(err)
    }
}

onMounted(() => {
    carregarModalidades()
})
</script>
