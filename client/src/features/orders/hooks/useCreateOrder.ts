import { useMutation, useQueryClient } from '@tanstack/react-query'
import { ordersApi } from '../api/ordersApi'

export function useCreateOrder() {
  const queryClient = useQueryClient()

  return useMutation({
    mutationFn: ordersApi.create,
    onSuccess: () => {
      // Criar um pedido tambem muda o estoque, entao invalidamos os dois.
      queryClient.invalidateQueries({ queryKey: ['orders'] })
      queryClient.invalidateQueries({ queryKey: ['stocks'] })
    },
  })
}
