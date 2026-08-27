import { useMutation, useQueryClient } from '@tanstack/react-query'
import { ordersApi } from '../api/ordersApi'

export function useCancelOrder() {
  const queryClient = useQueryClient()

  return useMutation({
    mutationFn: ordersApi.cancel,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['orders'] })
      queryClient.invalidateQueries({ queryKey: ['stocks'] })
    },
  })
}
