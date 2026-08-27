// Espelha ProductsCatalog.Application.Features.Books.BookDto / CreateBookCommand no backend.
export const BOOK_GENRES = [
  'Fiction', 'NonFiction', 'Mystery', 'Fantasy', 'ScienceFiction', 'Biography', 'SelfHelp',
  'Romance', 'Historical', 'Thriller', 'Horror', 'Poetry', 'YoungAdult', 'Children', 'Drama',
  'Adventure', 'GraphicNovel', 'Classic', 'Cookbook', 'Spirituality', 'Science', 'History',
  'Travel', 'Art', 'Psychology', 'Philosophy', 'Education', 'Music', 'Health', 'Business',
] as const

export type BookGenre = (typeof BOOK_GENRES)[number]

export interface Book {
  id: string
  price: number
  title: string
  author: string
  genre: number
  createdAt: string
}

export interface CreateBookInput {
  price: number
  title: string
  author: string
  genre: number
}
