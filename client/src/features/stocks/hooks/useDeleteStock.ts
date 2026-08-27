import { useMutation, useQueryClient } from '@tanstack/react-query'
import { stocksApi } from '../api/stocksApi'

export function useDeleteStock() {
  const queryClient = useQueryClient()

  return useMutation({
    mutationFn: stocksApi.remove,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['stocks'] })
    },
  })
}
