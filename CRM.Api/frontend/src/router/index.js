import { createRouter, createWebHistory } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

// Páginas e layout
import Login from '@/views/Login.vue'
import DefaultLayout from '@/layouts/DefaultLayout.vue'
import Dashboard from '@/views/Dashboard.vue'
import Usuarios from '@/views/UsuariosView.vue' 
import Alunos from '@/views/AlunoView.vue' 
import Modalidades from '@/views/ModalidadesView.vue'
import Cursos from '@/views/CursoView.vue'
import Matriculas from '@/views/CursoAlunoView.vue'
import Polos from '@/views/PoloView.vue'
import Pagamentos from '@/views/PagamentoView.vue'

const routes = [
  {
    path: '/',
    name: 'Login',
    component: Login,
    meta: { public: true }
  },
  {
    path: '/app',
    component: DefaultLayout,
    children: [
      { path: 'dashboard', name: 'Dashboard', component: Dashboard },
    { path: 'usuarios', name: 'Usuarios', component: Usuarios },
    { path: 'alunos', name: 'Alunos', component: Alunos },
    { path: 'curso', name: 'Cursos', component: Cursos },
    { path: 'modalidades', name: 'Modalidades', component: Modalidades },
    { path: 'Matricula', name: 'Matriculas', component: Matriculas },
    { path: 'Polo', name: 'Polos', component: Polos },
    { path: 'Pagamento', name: 'Pagamentos', component: Pagamentos }
      // futuras páginas:
      // { path: 'clientes', name: 'Clientes', component: () => import('@/views/Clientes.vue') },
    ]
  },
  // fallback
  { path: '/:pathMatch(.*)*', redirect: '/' }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

// Guarda de navegação: exige token nas rotas privadas
router.beforeEach((to) => {
  const auth = useAuthStore()

  // se está no login e já autenticado, manda pro dashboard
  if (to.meta.public && auth.isAuthenticated) {
    return { name: 'Dashboard' }
  }

  // se a rota NÃO é pública e não tem sessão válida, vai pro login
  if (!to.meta.public && !auth.isAuthenticated) {
    return { name: 'Login' }
  }

  return true
})

export default router
