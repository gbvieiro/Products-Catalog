import { useMutation, useQueryClient } from '@tanstack/react-query'
import { booksApi } from '../api/booksApi'

export function useDeleteBook() {
  const queryClient = useQueryClient()

  return useMutation({
    mutationFn: booksApi.remove,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['books'] })
      // A exclusao de um livro tambem invalida qualquer registro de estoque
      // associado no backend (ver StockConfiguration) - refletir isso aqui tambem.
      queryClient.invalidateQueries({ queryKey: ['stocks'] })
    },
  })
}
