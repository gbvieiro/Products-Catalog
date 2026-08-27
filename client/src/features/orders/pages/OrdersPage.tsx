import { StatusMessage } from '../../../shared/ui/StatusMessage'
import { getErrorMessage } from '../../../shared/api/httpClient'
import { useOrders } from '../hooks/useOrders'
import { OrdersTable } from '../components/OrdersTable'
import { CreateOrderForm } from '../components/CreateOrderForm'

export function OrdersPage() {
  const { data, isLoading, isError, error } = useOrders({ take: 50 })

  return (
    <section>
      <h1>Orders</h1>

      <CreateOrderForm />

      {isLoading && <StatusMessage kind="loading" />}
      {isError && <StatusMessage kind="error" message={getErrorMessage(error)} />}
      {data && data.items.length === 0 && <StatusMessage kind="empty" message="No orders yet." />}
      {data && data.items.length > 0 && <OrdersTable orders={data.items} />}
    </section>
  )
}
