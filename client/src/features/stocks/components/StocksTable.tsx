import type { Stock } from '../types'
import { useDeleteStock } from '../hooks/useDeleteStock'

interface StocksTableProps {
  stocks: Stock[]
  bookTitleById: Record<string, string>
  onEdit: (stock: Stock) => void
}

function statusOf(quantity: number): { label: string; tagClass: string } {
  if (quantity === 0) return { label: 'Out of stock', tagClass: 'tag tag-neutral' }
  if (quantity < 10) return { label: 'Low stock', tagClass: 'tag tag-outline' }
  return { label: 'In stock', tagClass: 'tag tag-accent' }
}

export function StocksTable({ stocks, bookTitleById, onEdit }: StocksTableProps) {
  const deleteStock = useDeleteStock()

  return (
    <table className="table">
      <thead>
        <tr>
          <th>Book</th>
          <th style={{ width: 130 }}>Quantity</th>
          <th style={{ width: 150 }}>Status</th>
          <th style={{ width: 170 }} />
        </tr>
      </thead>
      <tbody>
        {stocks.map((stock) => {
          const status = statusOf(stock.quantity)
          return (
            <tr key={stock.id}>
              <td style={{ fontWeight: 600 }} title={stock.bookId}>
                {bookTitleById[stock.bookId] ?? stock.bookId.slice(0, 8)}
              </td>
              <td style={{ fontFamily: 'ui-monospace, Menlo, monospace' }}>{stock.quantity}</td>
              <td><span className={status.tagClass}>{status.label}</span></td>
              <td style={{ textAlign: 'right', display: 'flex', gap: 4, justifyContent: 'flex-end' }}>
                <button type="button" className="btn btn-ghost rowact" onClick={() => onEdit(stock)}>
                  Edit
                </button>
                <button
                  type="button"
                  className="btn btn-ghost rowact"
                  disabled={deleteStock.isPending}
                  onClick={() => {
                    if (confirm('Delete this stock record?')) {
                      deleteStock.mutate(stock.bookId)
                    }
                  }}
                >
                  Delete
                </button>
              </td>
            </tr>
          )
        })}
      </tbody>
    </table>
  )
}
