import { httpClient } from '../../../shared/api/httpClient'
import type { PagedResult } from '../../../shared/types/pagination'
import type { CancelOrderResult, CreateOrderInput, Order } from '../types'

export const ordersApi = {
  list: async (params?: { filter?: string; skip?: number; take?: number }) => {
    const { data } = await httpClient.get<PagedResult<Order>>('/orders', { params })
    return data
  },

  getById: async (id: string) => {
    const { data } = await httpClient.get<Order>(`/orders/${id}`)
    return data
  },

  create: async (input: CreateOrderInput) => {
    const { data } = await httpClient.post<string>('/orders', input)
    return data
  },

  cancel: async (id: string) => {
    const { data } = await httpClient.put<CancelOrderResult>(`/orders/${id}/cancel`)
    return data
  },
}
