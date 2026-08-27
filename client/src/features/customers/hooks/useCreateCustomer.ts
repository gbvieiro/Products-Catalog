import { useMutation, useQueryClient } from '@tanstack/react-query'
import { customersApi } from '../api/customersApi'

export function useCreateCustomer() {
  const queryClient = useQueryClient()

  return useMutation({
    mutationFn: customersApi.create,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['customers'] })
    },
  })
}
