import { useQuery } from '@tanstack/react-query'
import { customersApi } from '../api/customersApi'

export function useCustomers(params?: { filter?: string; skip?: number; take?: number }) {
  return useQuery({
    queryKey: ['customers', params],
    queryFn: () => customersApi.list(params),
  })
}
