import { httpClient } from '../../../shared/api/httpClient'
import type { PagedResult } from '../../../shared/types/pagination'
import type { Customer, CreateCustomerInput, UpdateCustomerInput } from '../types'

export const customersApi = {
  list: async (params?: { filter?: string; skip?: number; take?: number }) => {
    const { data } = await httpClient.get<PagedResult<Customer>>('/customers', { params })
    return data
  },

  getById: async (id: string) => {
    const { data } = await httpClient.get<Customer>(`/customers/${id}`)
    return data
  },

  create: async (input: CreateCustomerInput) => {
    const { data } = await httpClient.post<string>('/customers', input)
    return data
  },

  update: async (id: string, input: UpdateCustomerInput) => {
    await httpClient.put(`/customers/${id}`, input)
  },

  remove: async (id: string) => {
    await httpClient.delete(`/customers/${id}`)
  },
}
