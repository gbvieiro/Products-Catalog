// Espelha ProductsCatalog.Application.Features.Stocks.* no backend.
import type { Book } from '../books/types'

export interface Stock {
  id: string
  bookId: string
  quantity: number
}

/** Stock "rico", com os dados do livro embutidos (retornado por GET /stocks/book/{bookId}). */
export interface CompleteStock extends Stock {
  book: Book
}

export interface CreateStockInput {
  bookId: string
  quantity: number
}

export interface UpdateStockInput {
  quantity: number
}
