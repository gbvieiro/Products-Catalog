import { type FormEvent, useEffect, useState } from 'react'
import { useBooks } from '../../books/hooks/useBooks'
import { useCreateStock } from '../hooks/useCreateStock'
import { useUpdateStock } from '../hooks/useUpdateStock'
import { getErrorMessage } from '../../../shared/api/httpClient'
import { StatusMessage } from '../../../shared/ui/StatusMessage'
import { Modal } from '../../../shared/ui/Modal'
import type { Stock } from '../types'

interface StockFormProps {
  /** Quando presente, o form edita a quantidade deste estoque em vez de criar um novo. */
  editingStock?: Stock | null
  onClose: () => void
}

/**
 * Um unico form (em Modal) para criar o estoque de um livro e editar a
 * quantidade de um estoque existente (troca de modo via `editingStock`). O
 * livro so pode ser escolhido na criacao: o registro de estoque e
 * identificado pelo bookId, e "trocar de livro" na edicao equivaleria a
 * apagar um registro e criar outro.
 */
export function StockForm({ editingStock, onClose }: StockFormProps) {
  const { data: booksPage } = useBooks({ take: 100 })
  const createStock = useCreateStock()
  const updateStock = useUpdateStock()

  const [bookId, setBookId] = useState('')
  const [quantity, setQuantity] = useState('0')

  const isEditing = Boolean(editingStock)
  const books = booksPage?.items ?? []
  const mutation = isEditing ? updateStock : createStock

  useEffect(() => {
    setBookId(editingStock?.bookId ?? '')
    setQuantity(editingStock ? String(editingStock.quantity) : '0')
  }, [editingStock])

  const handleSubmit = (event: FormEvent) => {
    event.preventDefault()
    const parsedQuantity = Number(quantity)

    if (isEditing && editingStock) {
      updateStock.mutate(
        { bookId: editingStock.bookId, input: { quantity: parsedQuantity } },
        { onSuccess: () => onClose() },
      )
      return
    }

    if (!bookId) return
    createStock.mutate({ bookId, quantity: parsedQuantity }, { onSuccess: () => onClose() })
  }

  return (
    <Modal title={isEditing ? 'Edit stock record' : 'New stock record'} onClose={onClose}>
      <form onSubmit={handleSubmit} style={{ display: 'flex', flexDirection: 'column', gap: 16 }}>
        <div className="dialog-body">
          <div className="field">
            <label htmlFor="bookId">Book</label>
            <select
              id="bookId"
              value={bookId}
              onChange={(e) => setBookId(e.target.value)}
              disabled={isEditing}
              required
            >
              <option value="">Select a book...</option>
              {books.map((book) => (
                <option key={book.id} value={book.id}>{book.title}</option>
              ))}
            </select>
          </div>

          <div className="field">
            <label htmlFor="quantity">Quantity</label>
            <input
              id="quantity"
              type="number"
              min={0}
              value={quantity}
              onChange={(e) => setQuantity(e.target.value)}
              required
            />
          </div>

          {mutation.isError && <StatusMessage kind="error" message={getErrorMessage(mutation.error)} />}
        </div>

        <div className="dialog-actions">
          <button type="button" className="btn btn-secondary" onClick={onClose}>Cancel</button>
          <button type="submit" className="btn btn-primary" disabled={mutation.isPending}>
            {isEditing
              ? mutation.isPending ? 'Saving...' : 'Save changes'
              : mutation.isPending ? 'Creating...' : 'Create record'}
          </button>
        </div>
      </form>
    </Modal>
  )
}
