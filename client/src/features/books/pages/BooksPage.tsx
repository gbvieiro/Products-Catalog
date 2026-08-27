import { type FormEvent, useState } from 'react'
import { StatusMessage } from '../../../shared/ui/StatusMessage'
import { getErrorMessage } from '../../../shared/api/httpClient'
import { useBooks } from '../hooks/useBooks'
import { useCreateBook } from '../hooks/useCreateBook'
import { BOOK_GENRES } from '../types'

export function BooksPage() {
  const { data, isLoading, isError, error } = useBooks({ take: 50 })
  const createBook = useCreateBook()

  const [title, setTitle] = useState('')
  const [author, setAuthor] = useState('')
  const [price, setPrice] = useState('')
  const [genre, setGenre] = useState(0)

  const handleSubmit = (event: FormEvent) => {
    event.preventDefault()
    createBook.mutate(
      { title, author, price: Number(price), genre },
      { onSuccess: () => { setTitle(''); setAuthor(''); setPrice('') } },
    )
  }

  return (
    <section>
      <h1>Books</h1>

      <form onSubmit={handleSubmit}>
        <div className="field">
          <label htmlFor="title">Title</label>
          <input id="title" value={title} onChange={(e) => setTitle(e.target.value)} required maxLength={30} />
        </div>
        <div className="field">
          <label htmlFor="author">Author</label>
          <input id="author" value={author} onChange={(e) => setAuthor(e.target.value)} required maxLength={30} />
        </div>
        <div className="field">
          <label htmlFor="price">Price</label>
          <input
            id="price"
            type="number"
            min={0}
            step="0.01"
            value={price}
            onChange={(e) => setPrice(e.target.value)}
            required
          />
        </div>
        <div className="field">
          <label htmlFor="genre">Genre</label>
          <select id="genre" value={genre} onChange={(e) => setGenre(Number(e.target.value))}>
            {BOOK_GENRES.map((label, index) => (
              <option key={label} value={index}>
                {label}
              </option>
            ))}
          </select>
        </div>
        <button type="submit" disabled={createBook.isPending}>
          {createBook.isPending ? 'Creating...' : 'Create book'}
        </button>
      </form>

      {createBook.isError && <StatusMessage kind="error" message={getErrorMessage(createBook.error)} />}

      {isLoading && <StatusMessage kind="loading" />}
      {isError && <StatusMessage kind="error" message={getErrorMessage(error)} />}

      {data && data.items.length === 0 && <StatusMessage kind="empty" message="No books yet." />}

      {data && data.items.length > 0 && (
        <table>
          <thead>
            <tr>
              <th>Title</th>
              <th>Author</th>
              <th>Price</th>
            </tr>
          </thead>
          <tbody>
            {data.items.map((book) => (
              <tr key={book.id}>
                <td>{book.title}</td>
                <td>{book.author}</td>
                <td>${book.price.toFixed(2)}</td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </section>
  )
}
