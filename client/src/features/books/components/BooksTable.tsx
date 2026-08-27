import type { Book } from '../types'
import { BOOK_GENRES } from '../types'
import { useDeleteBook } from '../hooks/useDeleteBook'

interface BooksTableProps {
  books: Book[]
}

export function BooksTable({ books }: BooksTableProps) {
  const deleteBook = useDeleteBook()

  return (
    <table className="table">
      <thead>
        <tr>
          <th>Title</th>
          <th style={{ width: 220 }}>Author</th>
          <th style={{ width: 160 }}>Genre</th>
          <th style={{ width: 110 }}>Price</th>
          <th style={{ width: 90 }} />
        </tr>
      </thead>
      <tbody>
        {books.map((book) => (
          <tr key={book.id}>
            <td style={{ fontWeight: 600 }}>{book.title}</td>
            <td>{book.author}</td>
            <td><span className="tag tag-neutral">{BOOK_GENRES[book.genre] ?? book.genre}</span></td>
            <td>${book.price.toFixed(2)}</td>
            <td style={{ textAlign: 'right' }}>
              <button
                type="button"
                className="btn btn-ghost rowact"
                disabled={deleteBook.isPending}
                onClick={() => {
                  if (confirm(`Delete "${book.title}"?`)) {
                    deleteBook.mutate(book.id)
                  }
                }}
              >
                Delete
              </button>
            </td>
          </tr>
        ))}
      </tbody>
    </table>
  )
}
