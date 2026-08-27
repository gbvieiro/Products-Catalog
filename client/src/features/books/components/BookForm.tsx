import { type FormEvent, useState } from 'react'
import { useCreateBook } from '../hooks/useCreateBook'
import { getErrorMessage } from '../../../shared/api/httpClient'
import { StatusMessage } from '../../../shared/ui/StatusMessage'
import { Modal } from '../../../shared/ui/Modal'
import { BOOK_GENRES } from '../types'

interface BookFormProps {
  onClose: () => void
}

export function BookForm({ onClose }: BookFormProps) {
  const createBook = useCreateBook()

  const [title, setTitle] = useState('')
  const [author, setAuthor] = useState('')
  const [price, setPrice] = useState('')
  const [genre, setGenre] = useState(0)

  const handleSubmit = (event: FormEvent) => {
    event.preventDefault()
    createBook.mutate({ title, author, price: Number(price), genre }, { onSuccess: () => onClose() })
  }

  return (
    <Modal title="New book" onClose={onClose}>
      <form onSubmit={handleSubmit} style={{ display: 'flex', flexDirection: 'column', gap: 16 }}>
        <div className="dialog-body" style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: 14 }}>
          <div className="field" style={{ gridColumn: '1 / -1' }}>
            <label htmlFor="title">Title</label>
            <input id="title" value={title} onChange={(e) => setTitle(e.target.value)} required maxLength={30} />
          </div>
          <div className="field" style={{ gridColumn: '1 / -1' }}>
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
                <option key={label} value={index}>{label}</option>
              ))}
            </select>
          </div>

          {createBook.isError && <StatusMessage kind="error" message={getErrorMessage(createBook.error)} />}
        </div>

        <div className="dialog-actions">
          <button type="button" className="btn btn-secondary" onClick={onClose}>Cancel</button>
          <button type="submit" className="btn btn-primary" disabled={createBook.isPending}>
            {createBook.isPending ? 'Creating...' : 'Create book'}
          </button>
        </div>
      </form>
    </Modal>
  )
}
