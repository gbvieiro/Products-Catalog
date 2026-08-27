import type { Order } from '../types'
import { ORDER_STATUS } from '../types'
import { useCancelOrder } from '../hooks/useCancelOrder'
import { useAuth } from '../../../shared/auth/AuthContext'
import { Role } from '../../../shared/types/role'

interface OrdersTableProps {
  orders: Order[]
  customerNameById: Record<string, string>
}

// Confirmed = destaque (accent), Canceled = neutro, Created (default) = outline.
function statusTagClass(status: Order['status']): string {
  if (status === 2) return 'tag tag-accent'
  if (status === 3) return 'tag tag-neutral'
  return 'tag tag-outline'
}

export function OrdersTable({ orders, customerNameById }: OrdersTableProps) {
  const cancelOrder = useCancelOrder()
  const { hasRole } = useAuth()
  // So o Administrator pode cancelar pedidos (ver PUT /api/orders/{id}/cancel no backend, restrito a Administrator).
  const canCancel = hasRole(Role.Administrator)

  return (
    <table className="table">
      <thead>
        <tr>
          <th style={{ width: 110 }}>Order</th>
          <th>Customer</th>
          <th style={{ width: 130 }}>Status</th>
          <th style={{ width: 90 }}>Items</th>
          <th style={{ width: 110 }}>Total</th>
          <th style={{ width: 110 }} />
        </tr>
      </thead>
      <tbody>
        {orders.map((order) => (
          <tr key={order.id}>
            <td title={order.id} style={{ fontFamily: 'ui-monospace, Menlo, monospace', fontSize: 13 }}>
              {order.id.slice(0, 8)}
            </td>
            <td style={{ fontWeight: 600 }} title={order.customerId}>
              {customerNameById[order.customerId] ?? order.customerId.slice(0, 8)}
            </td>
            <td><span className={statusTagClass(order.status)}>{ORDER_STATUS[order.status]}</span></td>
            <td>{order.items.reduce((sum, item) => sum + item.quantity, 0)}</td>
            <td>${order.totalAmount.toFixed(2)}</td>
            <td style={{ textAlign: 'right' }}>
              {canCancel && order.status !== 3 && (
                <button
                  type="button"
                  className="btn btn-ghost rowact"
                  disabled={cancelOrder.isPending}
                  onClick={() => cancelOrder.mutate(order.id)}
                >
                  Cancel
                </button>
              )}
            </td>
          </tr>
        ))}
      </tbody>
    </table>
  )
}
