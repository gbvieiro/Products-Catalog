import { useState } from 'react'
import { StatusMessage } from '../../../shared/ui/StatusMessage'
import { PageHeader } from '../../../shared/ui/PageHeader'
import { SearchIcon } from '../../../shared/ui/icons'
import { getErrorMessage } from '../../../shared/api/httpClient'
import { useOrders } from '../hooks/useOrders'
import { useCustomers } from '../../customers/hooks/useCustomers'
import { OrdersTable } from '../components/OrdersTable'
import { CreateOrderForm } from '../components/CreateOrderForm'

export function OrdersPage() {
  const [query, setQuery] = useState('')
  const [isCreateOpen, setIsCreateOpen] = useState(false)

  const { data, isLoading, isError, error } = useOrders({ filter: query || undefined, take: 50 })
  const { data: customersPage } = useCustomers({ take: 100 })

  const customerNameById = Object.fromEntries(
    (customersPage?.items ?? []).map((customer) => [customer.id, customer.name]),
  )

  return (
    <>
      <PageHeader title="Orders" action={{ label: 'New order', onClick: () => setIsCreateOpen(true) }} />
      <div className="page-content">
        <div className="list-toolbar">
          <div className="search-field">
            <input
              className="input"
              placeholder="Search by order or customer"
              value={query}
              onChange={(e) => setQuery(e.target.value)}
            />
            <SearchIcon />
          </div>
          {data && <span className="list-count">{data.totalCount} order{data.totalCount === 1 ? '' : 's'}</span>}
        </div>

        {isLoading && <StatusMessage kind="loading" />}
        {isError && <StatusMessage kind="error" message={getErrorMessage(error)} />}
        {data && data.items.length === 0 && <StatusMessage kind="empty" message="No orders match this search." />}
        {data && data.items.length > 0 && (
          <OrdersTable orders={data.items} customerNameById={customerNameById} />
        )}
      </div>

      {isCreateOpen && <CreateOrderForm onClose={() => setIsCreateOpen(false)} />}
    </>
  )
}
