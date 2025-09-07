import { fileURLToPath, URL } from 'node:url'
import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'

export default defineConfig({
  plugins: [vue()],
  resolve: {
    alias: { '@': fileURLToPath(new URL('./src', import.meta.url)) }
  },
  // 🔧 saída do build direto no wwwroot da API
  build: {
    outDir: '../wwwroot',
    emptyOutDir: true
  },
  // 🔄 servidor de desenvolvimento acessível via IP/nome da máquina
  server: {
    host: true, // permite acessar usando o IP da máquina
    port: 5173,
    proxy: {
      '/api': {
        target: 'http://192.168.0.10:5000', // coloque o IP/nome do servidor + porta da API
        changeOrigin: true,
        secure: false
      },
      '/login': {
        target: 'http://192.168.0.10:5000',
        changeOrigin: true,
        secure: false
      }
    }
  }
})
