import { useState } from 'react'
import { StatusMessage } from '../../../shared/ui/StatusMessage'
import { PageHeader } from '../../../shared/ui/PageHeader'
import { SearchIcon } from '../../../shared/ui/icons'
import { getErrorMessage } from '../../../shared/api/httpClient'
import { useUsers } from '../hooks/useUsers'
import { UserForm } from '../components/UserForm'
import { UsersTable } from '../components/UsersTable'
import type { User } from '../types'

export function UsersPage() {
  const [query, setQuery] = useState('')
  const [editingUser, setEditingUser] = useState<User | null>(null)
  const [isCreateOpen, setIsCreateOpen] = useState(false)

  const { data, isLoading, isError, error } = useUsers({ filter: query || undefined, take: 50 })

  const isModalOpen = isCreateOpen || editingUser !== null
  const closeModal = () => {
    setIsCreateOpen(false)
    setEditingUser(null)
  }

  return (
    <>
      <PageHeader title="Users" action={{ label: 'New user', onClick: () => setIsCreateOpen(true) }} />
      <div className="page-content">
        <div className="list-toolbar">
          <div className="search-field">
            <input
              className="input"
              placeholder="Search by email"
              value={query}
              onChange={(e) => setQuery(e.target.value)}
            />
            <SearchIcon />
          </div>
          {data && <span className="list-count">{data.totalCount} user{data.totalCount === 1 ? '' : 's'}</span>}
        </div>

        {isLoading && <StatusMessage kind="loading" />}
        {isError && <StatusMessage kind="error" message={getErrorMessage(error)} />}
        {data && data.items.length === 0 && <StatusMessage kind="empty" message="No users match this search." />}
        {data && data.items.length > 0 && (
          <UsersTable users={data.items} onEdit={setEditingUser} />
        )}
      </div>

      {isModalOpen && <UserForm editingUser={editingUser} onClose={closeModal} />}
    </>
  )
}
