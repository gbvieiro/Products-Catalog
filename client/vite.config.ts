import react from '@vitejs/plugin-react'
import { defineConfig } from 'vite'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    proxy: {
      // Evita problema de CORS em dev: o client chama "/api/..." e o Vite
      // repassa para a Api real. Em producao, defina VITE_API_BASE_URL
      // (ver src/shared/api/httpClient.ts) apontando direto para a Api.
      '/api': {
        target: process.env.VITE_API_PROXY_TARGET ?? 'http://localhost:5145',
        changeOrigin: true,
      },
    },
  },
})
