import { useState } from 'react'
import { StatusMessage } from '../../../shared/ui/StatusMessage'
import { PageHeader } from '../../../shared/ui/PageHeader'
import { SearchIcon } from '../../../shared/ui/icons'
import { getErrorMessage } from '../../../shared/api/httpClient'
import { useStocks } from '../hooks/useStocks'
import { useBooks } from '../../books/hooks/useBooks'
import { StockForm } from '../components/StockForm'
import { StocksTable } from '../components/StocksTable'
import type { Stock } from '../types'

export function StocksPage() {
  const [query, setQuery] = useState('')
  const [editingStock, setEditingStock] = useState<Stock | null>(null)
  const [isCreateOpen, setIsCreateOpen] = useState(false)

  const { data, isLoading, isError, error } = useStocks({ filter: query || undefined, take: 50 })
  const { data: booksPage } = useBooks({ take: 100 })

  const bookTitleById = Object.fromEntries(
    (booksPage?.items ?? []).map((book) => [book.id, book.title]),
  )

  const isModalOpen = isCreateOpen || editingStock !== null
  const closeModal = () => {
    setIsCreateOpen(false)
    setEditingStock(null)
  }

  return (
    <>
      <PageHeader title="Stocks" action={{ label: 'New stock', onClick: () => setIsCreateOpen(true) }} />
      <div className="page-content">
        <div className="list-toolbar">
          <div className="search-field">
            <input
              className="input"
              placeholder="Search by book"
              value={query}
              onChange={(e) => setQuery(e.target.value)}
            />
            <SearchIcon />
          </div>
          {data && <span className="list-count">{data.totalCount} record{data.totalCount === 1 ? '' : 's'}</span>}
        </div>

        {isLoading && <StatusMessage kind="loading" />}
        {isError && <StatusMessage kind="error" message={getErrorMessage(error)} />}
        {data && data.items.length === 0 && <StatusMessage kind="empty" message="No stock records match this search." />}
        {data && data.items.length > 0 && (
          <StocksTable stocks={data.items} bookTitleById={bookTitleById} onEdit={setEditingStock} />
        )}
      </div>

      {isModalOpen && <StockForm editingStock={editingStock} onClose={closeModal} />}
    </>
  )
}
