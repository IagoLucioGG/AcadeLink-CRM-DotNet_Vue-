<template>
  <v-container fluid>
    <h1 class="text-h4 mb-6 font-weight-bold">Dashboard Acadêmico</h1>

    <v-alert dense type="warning" v-if="useMock" class="mb-6">
      Não foi possível carregar dados da API — exibindo exemplos mock.
    </v-alert>

    <!-- Linha 1: Totais e percentuais -->
    <v-row dense class="mb-6" align="stretch">
      <v-col cols="12" md="3" v-for="card in cardsPrincipais" :key="card.title">
        <v-card outlined class="pa-4">
          <v-card-title>{{ card.title }}</v-card-title>
          <v-card-text class="text-h3 font-weight-bold">{{ card.value }}</v-card-text>
          <v-progress-linear v-if="card.percentual !== undefined" :value="card.percentual" height="12"
            :color="card.color" rounded></v-progress-linear>
          <small v-if="card.percentual !== undefined">{{ card.percentual.toFixed(1) }}%</small>
        </v-card>
      </v-col>
    </v-row>

    <!-- Linha 2: Alunos por polo e curso -->
    <v-row dense class="mb-6">
      <v-col cols="12" md="6">
        <v-card outlined class="pa-4">
          <v-card-title>Alunos por Polo</v-card-title>
          <v-card-text>
            <div v-for="(qtd, idx) in alunosPorPolo" :key="idx" class="mb-2">
              <div class="d-flex justify-space-between">
                <span>{{ poloLabels[idx] }}</span>
                <span>{{ qtd }}</span>
              </div>
              <v-progress-linear :value="(qtd / totalAlunos) * 100" height="10" color="purple"
                rounded></v-progress-linear>
            </div>
          </v-card-text>
        </v-card>
      </v-col>

      <v-col cols="12" md="6">
        <v-card outlined class="pa-4">
          <v-card-title>Alunos por Curso</v-card-title>
          <v-card-text>
            <div v-for="(qtd, idx) in alunosPorCurso" :key="idx" class="mb-2">
              <div class="d-flex justify-space-between">
                <span>{{ cursoLabels[idx] }}</span>
                <span>{{ qtd }}</span>
              </div>
              <v-progress-linear :value="(qtd / totalAlunos) * 100" height="10" color="orange"
                rounded></v-progress-linear>
            </div>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>

    <!-- Linha 3: Matrículas ativas/inativas -->
    <v-row dense class="mb-6">
      <v-col cols="12" md="6">
        <v-card outlined class="pa-4">
          <v-card-title>Matrículas Ativas vs Inativas</v-card-title>
          <v-card-text class="d-flex justify-space-between">
            <span>Ativas: {{ matriculasAtivas }}</span>
            <span>Inativas: {{ matriculasInativas }}</span>
          </v-card-text>
          <v-progress-linear :value="percentualAtivos" height="20" color="green" rounded striped></v-progress-linear>
        </v-card>
      </v-col>

      <v-col cols="12" md="6">
        <v-card outlined class="pa-4">
          <v-card-title>Modalidades</v-card-title>
          <v-card-text>
            <div v-for="(qtd, idx) in matriculasPorModalidade" :key="idx" class="mb-2">
              <div class="d-flex justify-space-between">
                <span>{{ modalidadeLabels[idx] }}</span>
                <span>{{ qtd }}</span>
              </div>
              <v-progress-linear :value="(qtd / totalMatriculas) * 100" height="10" color="teal"
                rounded></v-progress-linear>
            </div>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>

    <!-- Linha 4: Matrículas recentes -->
    <v-row dense>
      <v-col cols="12" md="12">
        <v-card outlined class="pa-4">
          <v-card-title>Matrículas nos últimos 30 dias</v-card-title>
          <v-card-text>
            <div v-for="m in matriculasRecentes" :key="m.idCursoAluno">
              {{ m.aluno.nmAluno }} - {{ m.curso.nmCurso }} - {{ m.dataMatriculaObj.toLocaleDateString() }}
            </div>

          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue'
import api from '@/services/api'

// Dados
const alunos = ref([])
const matriculas = ref([])

const totalAlunos = ref(0)
const totalMatriculas = ref(0)
const matriculasAtivas = ref(0)
const matriculasInativas = ref(0)
const poloLabels = ref([])
const alunosPorPolo = ref([])
const cursoLabels = ref([])
const alunosPorCurso = ref([])
const modalidadeLabels = ref([])
const matriculasPorModalidade = ref([])
const qtdCursos = ref(0)
const useMock = ref(false)

// Percentual de alunos com matrícula ativa
const percentualAtivos = computed(() =>
  totalMatriculas.value ? (matriculasAtivas.value / totalMatriculas.value) * 100 : 0
)

// Cards principais
const cardsPrincipais = computed(() => [
  { title: 'Total de Alunos', value: totalAlunos.value, percentual: percentualAtivos.value, color: 'green' },
  { title: 'Total de Matrículas', value: totalMatriculas.value },
  { title: 'Matrículas Ativas', value: matriculasAtivas.value, percentual: percentualAtivos.value, color: 'green' },
  { title: 'Matrículas Inativas', value: matriculasInativas.value, percentual: 100 - percentualAtivos.value, color: 'red' },
  { title: 'Total de Cursos', value: qtdCursos.value }
])

const matriculasRecentes = computed(() => {
  const hoje = new Date()
  hoje.setHours(0, 0, 0, 0)
  return matriculas.value
    .map(m => ({
      ...m,
      dataMatriculaObj: new Date(m.dataMatricula) // converte string para Date
    }))
    .filter(m => {
      const diffDias = (hoje - m.dataMatriculaObj) / (1000 * 60 * 60 * 24)
      return diffDias >= 0 && diffDias <= 30
    })
})


console.log(matriculasRecentes);


// Carregar dados da API
async function carregarDados() {
  console.log(matriculasRecentes);
  useMock.value = false
  try {
    // 1️⃣ Pega todos os alunos
    const resAlunos = await api.get('/api/Aluno/listar')
    alunos.value = resAlunos.data.dados || []
    totalAlunos.value = alunos.value.length

    // 2️⃣ Pega todas as matrículas
    const resMatriculas = await api.get('/api/CursoAluno/listar')
    matriculas.value = resMatriculas.data.dados || []

    // Totais matrículas
    totalMatriculas.value = matriculas.value.length
    matriculasAtivas.value = matriculas.value.filter(m => !m.dataCancelamento).length
    matriculasInativas.value = matriculas.value.filter(m => m.dataCancelamento).length
    qtdCursos.value = new Set(matriculas.value.map(m => m.curso.idCurso)).size

    // Labels e contagens
    poloLabels.value = [...new Set(matriculas.value.map(m => m.polo.nmPolo))]
    alunosPorPolo.value = poloLabels.value.map(label =>
      matriculas.value.filter(m => m.polo.nmPolo === label && !m.dataCancelamento).length
    )

    cursoLabels.value = [...new Set(matriculas.value.map(m => m.curso.nmCurso))]
    alunosPorCurso.value = cursoLabels.value.map(label =>
      matriculas.value.filter(m => m.curso.nmCurso === label && !m.dataCancelamento).length
    )

    modalidadeLabels.value = [...new Set(matriculas.value.map(m => m.modalidade.nmModalidade))]
    matriculasPorModalidade.value = modalidadeLabels.value.map(label =>
      matriculas.value.filter(m => m.modalidade.nmModalidade === label && !m.dataCancelamento).length
    )

  } catch (err) {
    console.warn('Erro API, usando mock', err)
    useMock.value = true
  }
}

onMounted(carregarDados)
</script>


<style scoped>
/* Variáveis de cores */
:root {
  --color-primary: #5c6bc0;
  /* azul moderno */
  --color-secondary: #ff7043;
  /* laranja vibrante */
  --color-success: #43a047;
  /* verde */
  --color-warning: #f9a825;
  /* amarelo */
  --color-error: #e53935;
  /* vermelho */
  --color-info: #26c6da;
  /* teal */
  --color-bg: #f5f7fa;
  --color-card-bg: #ffffff;
  --color-text-primary: #212121;
  --color-text-secondary: #616161;
  --color-border: #e0e0e0;
  --font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
}

/* Reset básico */
* {
  box-sizing: border-box;
}

.v-container {
  background-color: var(--color-bg);
  min-height: 100vh;
  padding: 2rem 1rem;
  font-family: var(--font-family);
  color: var(--color-text-primary);
}

/* Título principal */
h1.text-h4 {
  font-weight: 700;
  font-size: 2.25rem;
  margin-bottom: 2rem;
  color: var(--color-primary);
  text-transform: uppercase;
  letter-spacing: 1.2px;
}

/* Alert */
.v-alert {
  border-radius: 8px;
  font-weight: 600;
  font-size: 0.95rem;
  background-color: #fff3e0 !important;
  color: var(--color-warning);
  border: 1px solid var(--color-warning);
  box-shadow: 0 2px 6px rgb(249 168 37 / 0.2);
}

/* Cards */
.v-card {
  background-color: var(--color-card-bg);
  border-radius: 12px;
  box-shadow: 0 4px 12px rgb(0 0 0 / 0.05);
  transition: box-shadow 0.3s ease;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  height: 100%;
  padding: 1.5rem;
}

.v-card:hover {
  box-shadow: 0 8px 24px rgb(0 0 0 / 0.12);
}

.v-card-title {
  font-weight: 700;
  font-size: 1.25rem;
  color: var(--color-primary);
  margin-bottom: 1rem;
  text-transform: uppercase;
  letter-spacing: 0.8px;
}

.v-card-text {
  font-weight: 700;
  font-size: 2.5rem;
  color: var(--color-text-primary);
  margin-bottom: 1rem;
  line-height: 1.1;
}

/* Progress Linear custom */
.v-progress-linear {
  border-radius: 8px !important;
  height: 14px !important;
  margin-bottom: 0.5rem !important;
  box-shadow: inset 0 1px 3px rgb(0 0 0 / 0.1);
}

/* Pequenos textos percentuais */
small {
  font-weight: 600;
  font-size: 0.85rem;
  color: var(--color-text-secondary);
}

/* Listas dentro dos cards (Alunos por Polo, Curso, Modalidades) */
.v-card-text>div {
  margin-bottom: 1rem;
}

.v-card-text>div:last-child {
  margin-bottom: 0;
}

.d-flex {
  display: flex !important;
}

.justify-space-between {
  justify-content: space-between !important;
}

.mb-2 {
  margin-bottom: 0.5rem !important;
}

.mb-6 {
  margin-bottom: 3rem !important;
}

/* Matrículas Ativas vs Inativas */
.v-card-text.d-flex {
  font-size: 1.1rem;
  font-weight: 600;
  color: var(--color-text-primary);
  margin-bottom: 1rem;
}

/* Matrículas recentes */
.v-card-text>div {
  font-size: 1rem;
  color: var(--color-text-secondary);
  padding: 0.25rem 0;
  border-bottom: 1px solid var(--color-border);
}

.v-card-text>div:last-child {
  border-bottom: none;
}

/* Responsividade */
@media (max-width: 960px) {
  .v-card-text {
    font-size: 2rem !important;
  }

  h1.text-h4 {
    font-size: 1.75rem;
  }
}

/* Cores customizadas para progress bars */
.v-progress-linear[color="green"] .v-progress-linear__bar {
  background-color: var(--color-success) !important;
}

.v-progress-linear[color="red"] .v-progress-linear__bar {
  background-color: var(--color-error) !important;
}

.v-progress-linear[color="purple"] .v-progress-linear__bar {
  background-color: #7e57c2 !important;
  /* roxo moderno */
}

.v-progress-linear[color="orange"] .v-progress-linear__bar {
  background-color: var(--color-secondary) !important;
}

.v-progress-linear[color="teal"] .v-progress-linear__bar {
  background-color: var(--color-info) !important;
}

/* Progress striped effect */
.v-progress-linear[striped] .v-progress-linear__bar {
  background-image: repeating-linear-gradient(45deg,
      rgba(255, 255, 255, 0.15),
      rgba(255, 255, 255, 0.15) 10px,
      transparent 10px,
      transparent 20px);
  animation: progress-stripes 1s linear infinite;
}

@keyframes progress-stripes {
  0% {
    background-position: 0 0;
  }

  100% {
    background-position: 40px 0;
  }
}
</style>