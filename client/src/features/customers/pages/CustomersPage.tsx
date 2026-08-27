import { useState } from 'react'
import { StatusMessage } from '../../../shared/ui/StatusMessage'
import { PageHeader } from '../../../shared/ui/PageHeader'
import { SearchIcon } from '../../../shared/ui/icons'
import { getErrorMessage } from '../../../shared/api/httpClient'
import { useCustomers } from '../hooks/useCustomers'
import { CustomerForm } from '../components/CustomerForm'
import { CustomersTable } from '../components/CustomersTable'
import type { Customer } from '../types'

export function CustomersPage() {
  const [query, setQuery] = useState('')
  const [editingCustomer, setEditingCustomer] = useState<Customer | null>(null)
  const [isCreateOpen, setIsCreateOpen] = useState(false)

  const { data, isLoading, isError, error } = useCustomers({ filter: query || undefined, take: 50 })

  const isModalOpen = isCreateOpen || editingCustomer !== null
  const closeModal = () => {
    setIsCreateOpen(false)
    setEditingCustomer(null)
  }

  return (
    <>
      <PageHeader title="Customers" action={{ label: 'New customer', onClick: () => setIsCreateOpen(true) }} />
      <div className="page-content">
        <div className="list-toolbar">
          <div className="search-field">
            <input
              className="input"
              placeholder="Search by name or email"
              value={query}
              onChange={(e) => setQuery(e.target.value)}
            />
            <SearchIcon />
          </div>
          {data && <span className="list-count">{data.totalCount} customer{data.totalCount === 1 ? '' : 's'}</span>}
        </div>

        {isLoading && <StatusMessage kind="loading" />}
        {isError && <StatusMessage kind="error" message={getErrorMessage(error)} />}
        {data && data.items.length === 0 && <StatusMessage kind="empty" message="No customers match this search." />}
        {data && data.items.length > 0 && (
          <CustomersTable customers={data.items} onEdit={setEditingCustomer} />
        )}
      </div>

      {isModalOpen && <CustomerForm editingCustomer={editingCustomer} onClose={closeModal} />}
    </>
  )
}
