import { useQuery } from '@tanstack/react-query'
import { stocksApi } from '../api/stocksApi'

export function useStocks(params?: { filter?: string; skip?: number; take?: number }) {
  return useQuery({
    queryKey: ['stocks', params],
    queryFn: () => stocksApi.list(params),
  })
}
