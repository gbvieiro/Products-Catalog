import { useQuery } from '@tanstack/react-query'
import { booksApi } from '../api/booksApi'

export function useBooks(params?: { filter?: string; skip?: number; take?: number }) {
  return useQuery({
    queryKey: ['books', params],
    queryFn: () => booksApi.list(params),
  })
}
