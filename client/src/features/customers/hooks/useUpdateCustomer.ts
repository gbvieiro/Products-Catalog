import { useMutation, useQueryClient } from '@tanstack/react-query'
import { customersApi } from '../api/customersApi'
import type { UpdateCustomerInput } from '../types'

export function useUpdateCustomer() {
  const queryClient = useQueryClient()

  return useMutation({
    mutationFn: ({ id, input }: { id: string; input: UpdateCustomerInput }) => customersApi.update(id, input),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['customers'] })
    },
  })
}
