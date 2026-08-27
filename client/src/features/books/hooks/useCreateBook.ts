import { useMutation, useQueryClient } from '@tanstack/react-query'
import { booksApi } from '../api/booksApi'

export function useCreateBook() {
  const queryClient = useQueryClient()

  return useMutation({
    mutationFn: booksApi.create,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['books'] })
    },
  })
}
