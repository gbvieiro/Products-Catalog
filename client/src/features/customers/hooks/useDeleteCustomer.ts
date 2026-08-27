import { useMutation, useQueryClient } from '@tanstack/react-query'
import { customersApi } from '../api/customersApi'

export function useDeleteCustomer() {
  const queryClient = useQueryClient()

  return useMutation({
    mutationFn: customersApi.remove,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['customers'] })
    },
  })
}
