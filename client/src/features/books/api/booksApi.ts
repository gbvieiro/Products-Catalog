import { httpClient } from '../../../shared/api/httpClient'
import type { PagedResult } from '../../../shared/types/pagination'
import type { Book, CreateBookInput } from '../types'

export const booksApi = {
  list: async (params?: { filter?: string; skip?: number; take?: number }) => {
    const { data } = await httpClient.get<PagedResult<Book>>('/books', { params })
    return data
  },

  create: async (input: CreateBookInput) => {
    const { data } = await httpClient.post<string>('/books', input)
    return data
  },

  remove: async (id: string) => {
    await httpClient.delete(`/books/${id}`)
  },
}
