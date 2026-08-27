import { useMutation, useQueryClient } from '@tanstack/react-query'
import { usersApi } from '../api/usersApi'

export function useDeleteUser() {
  const queryClient = useQueryClient()

  return useMutation({
    mutationFn: usersApi.remove,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['users'] })
    },
  })
}
