import { type FormEvent, useState } from 'react'
import { Link } from 'react-router-dom'
import { useBooks } from '../../books/hooks/useBooks'
import { useCustomers } from '../../customers/hooks/useCustomers'
import { useCreateOrder } from '../hooks/useCreateOrder'
import { getErrorMessage } from '../../../shared/api/httpClient'
import { StatusMessage } from '../../../shared/ui/StatusMessage'
import { Modal } from '../../../shared/ui/Modal'
import { CloseIcon } from '../../../shared/ui/icons'
import type { CreateOrderItemInput } from '../types'

interface CreateOrderFormProps {
  onClose: () => void
}

/**
 * Formulario de criacao de pedido (dentro de um Modal): escolhe o cliente
 * (dropdown, ver features/customers/) e um ou mais livros com quantidade.
 * CreateOrderCommand exige um Customer existente no backend (ver
 * CreateOrderCommandHandler).
 */
export function CreateOrderForm({ onClose }: CreateOrderFormProps) {
  const { data: booksPage } = useBooks({ take: 100 })
  const { data: customersPage } = useCustomers({ take: 100 })
  const createOrder = useCreateOrder()

  const [customerId, setCustomerId] = useState('')
  const [items, setItems] = useState<CreateOrderItemInput[]>([{ bookId: '', quantity: 1 }])

  const books = booksPage?.items ?? []
  const customers = customersPage?.items ?? []
  const bookById = Object.fromEntries(books.map((book) => [book.id, book]))
  const total = items.reduce((sum, item) => sum + (bookById[item.bookId]?.price ?? 0) * item.quantity, 0)

  const updateItem = (index: number, patch: Partial<CreateOrderItemInput>) => {
    setItems((current) => current.map((item, i) => (i === index ? { ...item, ...patch } : item)))
  }

  const addItem = () => setItems((current) => [...current, { bookId: '', quantity: 1 }])
  const removeItem = (index: number) => setItems((current) => current.filter((_, i) => i !== index))

  const handleSubmit = (event: FormEvent) => {
    event.preventDefault()
    const validItems = items.filter((item) => item.bookId && item.quantity > 0)
    if (!customerId || validItems.length === 0) return

    createOrder.mutate({ customerId, items: validItems }, { onSuccess: () => onClose() })
  }

  return (
    <Modal title="New order" onClose={onClose}>
      <form onSubmit={handleSubmit} style={{ display: 'flex', flexDirection: 'column', gap: 16 }}>
        <div className="dialog-body">
          <div className="field">
            <label htmlFor="customerId">Customer</label>
            <select id="customerId" value={customerId} onChange={(e) => setCustomerId(e.target.value)} required>
              <option value="">Select a customer...</option>
              {customers.map((customer) => (
                <option key={customer.id} value={customer.id}>
                  {customer.name} ({customer.email})
                </option>
              ))}
            </select>
            {customers.length === 0 && (
              <span style={{ fontSize: 12 }} className="text-muted">
                No customers yet - <Link to="/customers" onClick={onClose}>create one</Link> first.
              </span>
            )}
          </div>

          <div>
            <label style={{ display: 'block', fontSize: 12, marginBottom: 5 }} className="text-muted">Items</label>
            <div style={{ display: 'flex', flexDirection: 'column', gap: 8 }}>
              {items.map((item, index) => (
                <div key={index} style={{ display: 'grid', gridTemplateColumns: '1fr 96px 36px', gap: 8, alignItems: 'center' }}>
                  <select
                    aria-label="Book"
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
                  <input
                    aria-label="Quantity"
                    type="number"
                    min={1}
                    value={item.quantity}
                    onChange={(e) => updateItem(index, { quantity: Number(e.target.value) })}
                    required
                  />
                  <button
                    type="button"
                    className="btn btn-secondary btn-icon"
                    onClick={() => removeItem(index)}
                    disabled={items.length === 1}
                    title="Remove item"
                  >
                    <CloseIcon style={{ width: 15, height: 15 }} />
                  </button>
                </div>
              ))}
            </div>
            <button type="button" className="btn btn-ghost" onClick={addItem} style={{ marginTop: 8 }}>
              + Add item
            </button>
          </div>

          <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'baseline', borderTop: '2px solid var(--color-divider)', paddingTop: 12 }}>
            <span style={{ fontSize: 11, letterSpacing: '.08em', textTransform: 'uppercase', opacity: 0.6 }}>Total</span>
            <span style={{ fontFamily: 'var(--font-heading)', fontWeight: 800, fontSize: 24 }}>${total.toFixed(2)}</span>
          </div>

          {createOrder.isError && <StatusMessage kind="error" message={getErrorMessage(createOrder.error)} />}
        </div>

        <div className="dialog-actions">
          <button type="button" className="btn btn-secondary" onClick={onClose}>Cancel</button>
          <button type="submit" className="btn btn-primary" disabled={createOrder.isPending}>
            {createOrder.isPending ? 'Placing order...' : 'Place order'}
          </button>
        </div>
      </form>
    </Modal>
  )
}
