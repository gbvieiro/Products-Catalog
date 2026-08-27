import { httpClient } from '../../../shared/api/httpClient'
import type { LoginInput, LoginResult } from '../types'

export const authApi = {
  login: async (input: LoginInput) => {
    const { data } = await httpClient.post<LoginResult>('/auth/login', input)
    return data
  },

  // JWT e stateless (ver LogoutCommand no backend) - este endpoint so existe
  // para completar o fluxo simetricamente; a limpeza de verdade (descartar o
  // token) acontece no client, em AuthContext.logout().
  logout: async () => {
    await httpClient.post('/auth/logout')
  },
}
