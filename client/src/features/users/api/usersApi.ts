import { httpClient } from '../../../shared/api/httpClient'
import type { PagedResult } from '../../../shared/types/pagination'
import type { CreateUserInput, UpdateUserInput, User } from '../types'

export const usersApi = {
  list: async (params?: { filter?: string; skip?: number; take?: number }) => {
    const { data } = await httpClient.get<PagedResult<User>>('/users', { params })
    return data
  },

  getById: async (id: string) => {
    const { data } = await httpClient.get<User>(`/users/${id}`)
    return data
  },

  create: async (input: CreateUserInput) => {
    const { data } = await httpClient.post<string>('/users', input)
    return data
  },

  update: async (id: string, input: UpdateUserInput) => {
    await httpClient.put(`/users/${id}`, input)
  },

  remove: async (id: string) => {
    await httpClient.delete(`/users/${id}`)
  },
}
