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
