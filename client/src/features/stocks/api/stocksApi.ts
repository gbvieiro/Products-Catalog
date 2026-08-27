import { httpClient } from '../../../shared/api/httpClient'
import type { PagedResult } from '../../../shared/types/pagination'
import type { CompleteStock, CreateStockInput, Stock, UpdateStockInput } from '../types'

export const stocksApi = {
  list: async (params?: { filter?: string; skip?: number; take?: number }) => {
    const { data } = await httpClient.get<PagedResult<Stock>>('/stocks', { params })
    return data
  },

  getByBookId: async (bookId: string) => {
    const { data } = await httpClient.get<CompleteStock>(`/stocks/book/${bookId}`)
    return data
  },

  create: async (input: CreateStockInput) => {
    const { data } = await httpClient.post<string>('/stocks', input)
    return data
  },

  // Ajuste administrativo: define a quantidade para um valor absoluto.
  // Ver PUT /api/stocks/book/{bookId}/add na Api para o fluxo de "somar"
  // (usado internamente ao repor estoque de fornecedor / cancelar pedido).
  update: async (bookId: string, input: UpdateStockInput) => {
    await httpClient.put(`/stocks/book/${bookId}`, input)
  },

  remove: async (bookId: string) => {
    await httpClient.delete(`/stocks/book/${bookId}`)
  },
}
