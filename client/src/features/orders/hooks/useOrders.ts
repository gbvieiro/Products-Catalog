import { useQuery } from '@tanstack/react-query'
import { ordersApi } from '../api/ordersApi'

export function useOrders(params?: { filter?: string; skip?: number; take?: number }) {
  return useQuery({
    queryKey: ['orders', params],
    queryFn: () => ordersApi.list(params),
  })
}
