import { useState } from 'react'
import { StatusMessage } from '../../../shared/ui/StatusMessage'
import { PageHeader } from '../../../shared/ui/PageHeader'
import { SearchIcon } from '../../../shared/ui/icons'
import { getErrorMessage } from '../../../shared/api/httpClient'
import { useBooks } from '../hooks/useBooks'
import { BooksTable } from '../components/BooksTable'
import { BookForm } from '../components/BookForm'

export function BooksPage() {
  const [query, setQuery] = useState('')
  const [isCreateOpen, setIsCreateOpen] = useState(false)

  const { data, isLoading, isError, error } = useBooks({ filter: query || undefined, take: 50 })

  return (
    <>
      <PageHeader title="Books" action={{ label: 'New book', onClick: () => setIsCreateOpen(true) }} />
      <div className="page-content">
        <div className="list-toolbar">
          <div className="search-field">
            <input
              className="input"
              placeholder="Search by title or author"
              value={query}
              onChange={(e) => setQuery(e.target.value)}
            />
            <SearchIcon />
          </div>
          {data && <span className="list-count">{data.totalCount} book{data.totalCount === 1 ? '' : 's'}</span>}
        </div>

        {isLoading && <StatusMessage kind="loading" />}
        {isError && <StatusMessage kind="error" message={getErrorMessage(error)} />}
        {data && data.items.length === 0 && <StatusMessage kind="empty" message="No books match this search." />}
        {data && data.items.length > 0 && <BooksTable books={data.items} />}
      </div>

      {isCreateOpen && <BookForm onClose={() => setIsCreateOpen(false)} />}
    </>
  )
}
