import { Link } from 'react-router-dom'
import { useAuth } from '../../../shared/auth/AuthContext'
import { Role } from '../../../shared/types/role'
import { BooksIcon, OrdersIcon, StocksIcon, UsersIcon } from '../../../shared/ui/icons'
import { PageHeader } from '../../../shared/ui/PageHeader'
import styles from './OverviewPage.module.css'

const WHAT_IT_DOES = [
  {
    icon: BooksIcon,
    title: 'Book catalog',
    body: 'Title, author, price and genre, validated by FluentValidation before the command reaches the domain.',
  },
  {
    icon: OrdersIcon,
    title: 'Orders',
    body: 'An order takes an existing customer and one or more books with quantities. Only an administrator can cancel one.',
  },
  {
    icon: StocksIcon,
    title: 'Stock control',
    body: 'A stock record per book, moved by domain events raised when orders are created and cancelled.',
  },
  {
    icon: UsersIcon,
    title: 'Access control',
    body: 'JWT login with two roles. The menu adapts, and the API returns 403 regardless of the UI.',
  },
]

const ARCHITECTURE_LAYERS = [
  { name: 'Domain', body: 'Entities, value objects, enums, specifications and domain events. No framework dependency.' },
  { name: 'Application', body: 'CQRS with MediatR — commands and queries plus pipeline behaviors. Depends only on Domain.' },
  { name: 'Infrastructure', body: 'EF Core persistence, repositories, caching and messaging. Depends on Application.' },
  { name: 'Api', body: 'Composition root: controllers, filters, middleware and Program.cs.' },
]

const PATTERNS = ['CQRS', 'MediatR', 'Specification', 'Unit of Work', 'Domain Events']

export function OverviewPage() {
  const { hasRole } = useAuth()
  const isAdmin = hasRole(Role.Administrator)

  return (
    <>
      <PageHeader title="Overview" />
      <div className="page-content">
        <div style={{ paddingBottom: 'var(--space-8)' }}>
          <div className={styles.kicker}>MVP · study project</div>
          <h1 className={styles.headline}>A products catalog built to show the architecture.</h1>
          <p className={styles.lead}>
            Products Catalog manages a catalog of books, order creation and stock control. It exists as a
            reference for Clean Architecture and CQRS with MediatR on .NET 8, with a React + TypeScript front
            end and a docker-compose that brings up Postgres, the API and the client together.
          </p>
          <div style={{ display: 'flex', gap: 8, marginTop: 24 }}>
            <Link to="/orders" className="btn btn-primary">Go to orders</Link>
            {isAdmin && (
              <Link to="/books" className="btn btn-secondary">Browse the catalog</Link>
            )}
          </div>
        </div>

        <hr className="hr" />
        <h6 style={{ margin: '0 0 20px' }}>What it does</h6>
        <div className={styles.whatGrid}>
          {WHAT_IT_DOES.map((item) => (
            <div key={item.title} className={styles.whatCard}>
              <item.icon style={{ width: 22, height: 22, marginBottom: 14 }} stroke="var(--color-accent)" strokeWidth={1.8} />
              <div className={styles.whatTitle}>{item.title}</div>
              <p className={styles.whatBody}>{item.body}</p>
            </div>
          ))}
        </div>

        <hr className="hr" style={{ margin: '40px 0' }} />
        <div className={styles.bottomGrid}>
          <div>
            <h6 style={{ margin: '0 0 20px' }}>Architecture</h6>
            <div className={styles.archList}>
              {ARCHITECTURE_LAYERS.map((layer) => (
                <div key={layer.name} className={styles.archRow}>
                  <div className={styles.archName}>{layer.name}</div>
                  <div className={styles.archBody}>{layer.body}</div>
                </div>
              ))}
            </div>
          </div>
          <div>
            <h6 style={{ margin: '0 0 20px' }}>Patterns</h6>
            <div style={{ display: 'flex', flexWrap: 'wrap', gap: 6 }}>
              {PATTERNS.map((pattern) => (
                <span key={pattern} className="tag tag-outline">{pattern}</span>
              ))}
            </div>
          </div>
        </div>

        <hr className="hr" style={{ margin: '40px 0' }} />
        <h6 style={{ margin: '0 0 20px' }}>Roles and permissions</h6>
        <table className="table" style={{ maxWidth: 900 }}>
          <thead>
            <tr>
              <th style={{ width: 200 }}>Role</th>
              <th>Can do</th>
            </tr>
          </thead>
          <tbody>
            <tr>
              <td style={{ fontWeight: 600 }}>Administrator</td>
              <td>Everything: CRUD on books, stock, customers and users, plus creating, viewing and cancelling orders.</td>
            </tr>
            <tr>
              <td style={{ fontWeight: 600 }}>Seller</td>
              <td>Only create and view orders. No access to books, stock, customers, users, or order cancellation.</td>
            </tr>
          </tbody>
        </table>
      </div>
    </>
  )
}
