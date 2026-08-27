import { useMutation, useQueryClient } from '@tanstack/react-query'
import { stocksApi } from '../api/stocksApi'
import type { UpdateStockInput } from '../types'

export function useUpdateStock() {
  const queryClient = useQueryClient()

  return useMutation({
    mutationFn: ({ bookId, input }: { bookId: string; input: UpdateStockInput }) => stocksApi.update(bookId, input),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['stocks'] })
    },
  })
}
