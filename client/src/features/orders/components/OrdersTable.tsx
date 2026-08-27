import type { Order } from '../types'
import { ORDER_STATUS } from '../types'
import { useCancelOrder } from '../hooks/useCancelOrder'

interface OrdersTableProps {
  orders: Order[]
}

export function OrdersTable({ orders }: OrdersTableProps) {
  const cancelOrder = useCancelOrder()

  return (
    <table>
      <thead>
        <tr>
          <th>Order</th>
          <th>Customer</th>
          <th>Status</th>
          <th>Items</th>
          <th>Total</th>
          <th />
        </tr>
      </thead>
      <tbody>
        {orders.map((order) => (
          <tr key={order.id}>
            <td title={order.id}>{order.id.slice(0, 8)}</td>
            <td title={order.customerId}>{order.customerId.slice(0, 8)}</td>
            <td>{ORDER_STATUS[order.status]}</td>
            <td>{order.items.reduce((sum, item) => sum + item.quantity, 0)}</td>
            <td>${order.totalAmount.toFixed(2)}</td>
            <td>
              {order.status !== 3 && (
                <button
                  type="button"
                  className="secondary"
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
