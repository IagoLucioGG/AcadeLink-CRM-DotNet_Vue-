<template>
  <v-app>
    <v-main class="login-bg">
      <v-container class="fill-height d-flex align-center justify-center">
        <v-card width="420" elevation="10" class="login-card pa-6">
          <!-- Logo e nome do sistema -->
          <div class="text-center mb-6">
            <v-icon color="primary" size="48">mdi-link-variant</v-icon>
            <h1 class="text-h5 font-weight-bold mt-2">AcadeLinkCRM</h1>
            <p class="text-subtitle-2 text-grey-darken-1">
              Conectando sua empresa ao sucesso
            </p>
          </div>

          <!-- Formulário -->
          <v-form @submit.prevent="onSubmit" v-model="valid">
            <v-text-field v-model="form.login" label="Usuário" prepend-inner-icon="mdi-account-outline"
              variant="outlined" density="comfortable" :rules="[v => !!v || 'Informe o login']" clearable />

            <v-text-field v-model="form.senha" label="Senha" type="password" prepend-inner-icon="mdi-lock-outline"
              variant="outlined" density="comfortable" :rules="[v => !!v || 'Informe a senha']" clearable />

            <v-btn type="submit" block color="primary" class="mt-4" size="large" :loading="loading">
              Entrar
            </v-btn>
          </v-form>

          <!-- Mensagem de erro -->
          <v-alert v-if="error" type="error" variant="tonal" class="mt-3" density="compact">
            {{ error }}
          </v-alert>

          <!-- Rodapé dentro do card -->
          <div class="text-center mt-6">
            <small class="text-grey-darken-1">
              © {{ new Date().getFullYear() }} AcadeLinkCRM — Desenvolvido por <strong>Iago Lucio</strong>
            </small>
          </div>
        </v-card>
      </v-container>

      <!-- Rodapé fixo na tela -->
      <v-footer app padless class="footer-bar">
        <v-col class="text-center text-white py-2">
          © {{ new Date().getFullYear() }} AcadeLinkCRM — Desenvolvido por <strong>Iago Lucio</strong>
        </v-col>
      </v-footer>
    </v-main>
  </v-app>
</template>

<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import api from '@/services/api'
import { useAuthStore } from '@/stores/auth'

const router = useRouter()
const auth = useAuthStore()

const form = ref({ login: '', senha: '' })
const valid = ref(true)
const loading = ref(false)
const error = ref('')

async function onSubmit() {
  error.value = ''
  if (!form.value.login || !form.value.senha) {
    error.value = 'Informe login e senha.'
    return
  }

  try {
    loading.value = true

    const { data } = await api.post('/login', {
      login: form.value.login,
      senha: form.value.senha
    })

    if (!data?.status) {
      error.value = data?.mensagem || 'Falha no login.'
      return
    }

    const token = data?.dados?.token
    const user = data?.dados?.usuario
    const empresa = data?.dados?.empresa

    if (!token) {
      error.value = 'Token não retornado pela API.'
      return
    }

    auth.setSession(token, user, empresa)
    router.push({ name: 'Dashboard' })
  } catch (e) {
    if (e.response) {
      const { mensagem } = e.response.data || {}
      // Verifica se a API informou limite de usuários
      if (mensagem?.includes('Limite de usuários simultâneos')) {
        error.value = mensagem
      } else {
        error.value = mensagem || 'Não foi possível autenticar. Verifique as credenciais.'
      }
    } else {
      error.value = 'Erro de conexão com o servidor.'
    }
  } finally {
    loading.value = false
  }
}
</script>


<style scoped>
.login-bg {
  background: linear-gradient(135deg, #1e88e5, #42a5f5);
}

.login-card {
  border-radius: 16px;
  background: white;
}

.footer-bar {
  background-color: #1e88e5;
  position: fixed;
  bottom: 0;
  width: 100%;
}
</style>
