import axios from 'axios'

/**
 * Instancia unica do axios usada por toda a aplicacao. Cada feature tem seu
 * proprio arquivo `api/xxxApi.ts` que usa este client (nunca axios direto),
 * assim endpoints, tratamento de erro e headers ficam centralizados aqui.
 */
export const httpClient = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL || '/api',
  headers: {
    'Content-Type': 'application/json',
  },
})

// Token JWT atual (ou null se deslogado). Guardado em memoria aqui e
// atualizado pelo AuthContext (que tambem persiste em localStorage) - o
// interceptor abaixo le sempre este valor, entao qualquer chamada feita via
// httpClient automaticamente carrega o header Authorization quando logado.
let currentToken: string | null = null

export function setAuthToken(token: string | null): void {
  currentToken = token
}

httpClient.interceptors.request.use((config) => {
  if (currentToken) {
    config.headers.Authorization = `Bearer ${currentToken}`
  }
  return config
})

// Callback registrado pelo AuthContext: disparado sempre que alguma resposta
// vier 401 (token ausente, expirado ou invalido), para forcar logout/redirect
// para a tela de login em um lugar so, em vez de cada feature tratar isso.
let unauthorizedHandler: (() => void) | null = null

export function setUnauthorizedHandler(handler: (() => void) | null): void {
  unauthorizedHandler = handler
}

httpClient.interceptors.response.use(
  (response) => response,
  (error) => {
    if (axios.isAxiosError(error) && error.response?.status === 401) {
      unauthorizedHandler?.()
    }
    return Promise.reject(error)
  },
)

export interface ProblemDetails {
  title?: string
  detail?: string
  status?: number
  errors?: Record<string, string[]>
}

export function getErrorMessage(error: unknown): string {
  if (axios.isAxiosError<ProblemDetails>(error)) {
    const problem = error.response?.data
    if (problem?.errors) {
      return Object.values(problem.errors).flat().join(' ')
    }
    return problem?.detail ?? problem?.title ?? error.message
  }
  return error instanceof Error ? error.message : 'Unexpected error'
}
