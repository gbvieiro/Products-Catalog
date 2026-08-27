import { useMutation, useQueryClient } from '@tanstack/react-query'
import { usersApi } from '../api/usersApi'

export function useCreateUser() {
  const queryClient = useQueryClient()

  return useMutation({
    mutationFn: usersApi.create,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['users'] })
    },
  })
}
