import { useQuery } from '@tanstack/react-query'
import { usersApi } from '../api/usersApi'

export function useUsers(params?: { filter?: string; skip?: number; take?: number }) {
  return useQuery({
    queryKey: ['users', params],
    queryFn: () => usersApi.list(params),
  })
}
