<template>
  <v-app>
    <!-- Menu lateral -->
    <v-navigation-drawer app :permanent="true" v-model="drawer" :rail="miniVariant" expand-on-hover class="drawer-bg">
      <!-- Cabeçalho -->
      <v-list-item class="drawer-header" :class="{ 'justify-center': miniVariant }">
        <v-avatar color="primary" size="40">
          <v-icon dark>mdi-link-variant</v-icon>
        </v-avatar>
        <div v-if="!miniVariant" class="ml-3">
          <div class="text-h6 font-weight-bold">AcadeLinkCRM</div>
          <div class="text-caption text-grey-lighten-1">
            {{ empresa?.nmEmpresa || 'Sua empresa' }}
          </div>
        </div>
      </v-list-item>

      <v-divider></v-divider>

      <!-- Menu -->
      <v-list density="comfortable" nav>
        <v-list-item :to="{ name: 'Dashboard' }" prepend-icon="mdi-view-dashboard-outline" link>
          <v-list-item-title>Dashboard</v-list-item-title>
        </v-list-item>

        <v-list-item :to="{ name: 'Usuarios' }" prepend-icon="mdi-account-multiple-outline" link>
          <v-list-item-title>Usuários</v-list-item-title>
        </v-list-item>

        <v-list-item :to="{ name: 'Alunos' }" prepend-icon="mdi-account-school-outline" link>
          <v-list-item-title>Alunos</v-list-item-title>
        </v-list-item>

        <v-list-item :to="{ name: 'Cursos' }" prepend-icon="mdi-school" link>
          <v-list-item-title>Cursos</v-list-item-title>
        </v-list-item>

        <v-list-item :to="{ name: 'Modalidades' }" prepend-icon="mdi-monitor" link>
          <v-list-item-title>Modalidades</v-list-item-title>
        </v-list-item>
        
        <v-list-item :to="{ name: 'Polos' }" prepend-icon="mdi-google-maps" link>
          <v-list-item-title>Polos</v-list-item-title>
        </v-list-item>

        <v-list-item :to="{ name: 'Matriculas' }" prepend-icon="mdi-library-shelves" link>
          <v-list-item-title>Matriculas</v-list-item-title>
        </v-list-item>

        <v-list-item :to="{ name: 'Pagamentos' }" prepend-icon="mdi-account-cash" link>
          <v-list-item-title>Pagamentos</v-list-item-title>
        </v-list-item>

        
      </v-list>

      <!-- Rodapé -->
      <template v-slot:append>
        <v-divider></v-divider>
        <div class="pa-4 text-center">
          <!-- Botão de Sair -->
          <v-btn block color="red-darken-2" variant="tonal" @click="logout">
            <v-icon>mdi-logout</v-icon>
            <span v-if="!miniVariant" class="ml-2">Sair</span>
          </v-btn>



          <!-- Botão de Expandir/Recolher -->
          <v-btn block variant="text" :icon="miniVariant" :prepend-icon="miniVariant ? '' : 'mdi-chevron-double-left'"
            @click="toggleMini">
            <template v-if="!miniVariant">
              {{ miniVariant ? 'Expandir' : 'Recolher' }}
            </template>
            <template v-else>
              <v-icon>mdi-chevron-double-right</v-icon>
            </template>
          </v-btn>

          <div v-if="!miniVariant" class="mt-4 text-caption text-grey-lighten-1">
            © {{ new Date().getFullYear() }} Desenvolvido por <strong>Iago Lucio</strong>
          </div>
        </div>
      </template>
    </v-navigation-drawer>

    <!-- Área principal -->
    <v-main>
      <v-container fluid class="py-6">
        <router-view />
      </v-container>
    </v-main>
  </v-app>
</template>
<script setup>
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import api from '@/services/api'
import { useAuthStore } from '@/stores/auth'

const auth = useAuthStore()
const router = useRouter()
const empresa = ref(null)

const drawer = ref(true)
const miniVariant = ref(false)

function toggleMini() {
  miniVariant.value = !miniVariant.value
}

onMounted(() => {
  empresa.value = JSON.parse(localStorage.getItem('empresa')) || null
})

async function logout() {
  try {
    await api.post('/logout') // seu endpoint real
  } catch (error) {
    console.error(
      'Erro ao tentar logout no servidor:',
      error.response?.data?.mensagem || error.message
    )
  } finally {
    // garante que localStorage e store são limpos
    auth.clearSession()
    localStorage.clear()
    sessionStorage.clear()

    // redireciona para login
    if (router.currentRoute.value.name !== 'Login') {
      router.push({ name: 'Login' })
    }
  }
}
</script>


<style scoped>
.drawer-bg {
  background: linear-gradient(180deg, #1e88e5, #1565c0);
  color: white;
}

.drawer-header {
  display: flex;
  align-items: center;
  padding: 16px;
  background-color: rgba(255, 255, 255, 0.05);
}

.v-list-item {
  color: white !important;
}

.v-list-item:hover {
  background-color: rgba(255, 255, 255, 0.08);
}

.v-icon {
  color: white !important;
}
</style>
