import { type FormEvent, useState } from 'react'
import { useBooks } from '../../books/hooks/useBooks'
import { useCreateOrder } from '../hooks/useCreateOrder'
import { getErrorMessage } from '../../../shared/api/httpClient'
import { StatusMessage } from '../../../shared/ui/StatusMessage'
import type { CreateOrderItemInput } from '../types'

/**
 * Formulario de criacao de pedido: escolhe o cliente (aqui simplificado para
 * um Guid digitado a mao, ja que nao ha login real - ver nota no backend
 * sobre autenticacao) e um ou mais livros com quantidade.
 */
export function CreateOrderForm() {
  const { data: booksPage } = useBooks({ take: 100 })
  const createOrder = useCreateOrder()

  const [customerId, setCustomerId] = useState('')
  const [items, setItems] = useState<CreateOrderItemInput[]>([{ bookId: '', quantity: 1 }])

  const books = booksPage?.items ?? []

  const updateItem = (index: number, patch: Partial<CreateOrderItemInput>) => {
    setItems((current) => current.map((item, i) => (i === index ? { ...item, ...patch } : item)))
  }

  const addItem = () => setItems((current) => [...current, { bookId: '', quantity: 1 }])
  const removeItem = (index: number) => setItems((current) => current.filter((_, i) => i !== index))

  const handleSubmit = (event: FormEvent) => {
    event.preventDefault()
    const validItems = items.filter((item) => item.bookId && item.quantity > 0)
    if (!customerId || validItems.length === 0) return

    createOrder.mutate(
      { customerId, items: validItems },
      { onSuccess: () => setItems([{ bookId: '', quantity: 1 }]) },
    )
  }

  return (
    <form onSubmit={handleSubmit} style={{ flexDirection: 'column', alignItems: 'stretch' }}>
      <div className="field">
        <label htmlFor="customerId">Customer ID (guid)</label>
        <input
          id="customerId"
          value={customerId}
          onChange={(e) => setCustomerId(e.target.value)}
          placeholder="00000000-0000-0000-0000-000000000000"
          required
        />
      </div>

      {items.map((item, index) => (
        <div key={index} style={{ display: 'flex', gap: '0.75rem', alignItems: 'flex-end' }}>
          <div className="field">
            <label htmlFor={`book-${index}`}>Book</label>
            <select
              id={`book-${index}`}
              value={item.bookId}
              onChange={(e) => updateItem(index, { bookId: e.target.value })}
              required
            >
              <option value="">Select a book...</option>
              {books.map((book) => (
                <option key={book.id} value={book.id}>
                  {book.title} (${book.price.toFixed(2)})
                </option>
              ))}
            </select>
          </div>
          <div className="field">
            <label htmlFor={`quantity-${index}`}>Quantity</label>
            <input
              id={`quantity-${index}`}
              type="number"
              min={1}
              value={item.quantity}
              onChange={(e) => updateItem(index, { quantity: Number(e.target.value) })}
              required
            />
          </div>
          {items.length > 1 && (
            <button type="button" className="secondary" onClick={() => removeItem(index)}>
              Remove
            </button>
          )}
        </div>
      ))}

      <div style={{ display: 'flex', gap: '0.75rem' }}>
        <button type="button" className="secondary" onClick={addItem}>
          + Add item
        </button>
        <button type="submit" disabled={createOrder.isPending}>
          {createOrder.isPending ? 'Placing order...' : 'Place order'}
        </button>
      </div>

      {createOrder.isError && <StatusMessage kind="error" message={getErrorMessage(createOrder.error)} />}
    </form>
  )
}
