import { useMutation, useQueryClient } from '@tanstack/react-query'
import { stocksApi } from '../api/stocksApi'

export function useCreateStock() {
  const queryClient = useQueryClient()

  return useMutation({
    mutationFn: stocksApi.create,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['stocks'] })
    },
  })
}
