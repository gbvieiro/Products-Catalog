import { type FormEvent, useState } from 'react'
import { Navigate, useLocation } from 'react-router-dom'
import { useAuth } from '../../../shared/auth/AuthContext'
import { getErrorMessage } from '../../../shared/api/httpClient'
import { StatusMessage } from '../../../shared/ui/StatusMessage'
import styles from './LoginPage.module.css'

export function LoginPage() {
  const { login, isAuthenticated } = useAuth()
  const location = useLocation()

  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [error, setError] = useState<unknown>(null)

  if (isAuthenticated) {
    const from = (location.state as { from?: Location })?.from?.pathname ?? '/'
    return <Navigate to={from} replace />
  }

  const handleSubmit = async (event: FormEvent) => {
    event.preventDefault()
    setIsSubmitting(true)
    setError(null)

    try {
      await login({ email, password })
    } catch (err) {
      setError(err)
    } finally {
      setIsSubmitting(false)
    }
  }

  return (
    <div className={styles.screen}>
      <div className={styles.intro}>
        <div className={styles.brand}>PRODUCTS CATALOG</div>
        <div>
          <h1 className={styles.headline}>Books, orders and stock in one place.</h1>
          <p className={styles.subhead}>
            An MVP built as a reference for Clean Architecture and CQRS on .NET 8, with a React front end.
          </p>
        </div>
        <div className={styles.tags}>
          <span className="tag tag-neutral">.NET 8</span>
          <span className="tag tag-neutral">MediatR</span>
          <span className="tag tag-neutral">EF Core</span>
          <span className="tag tag-neutral">React + TS</span>
        </div>
      </div>

      <div className={styles.formPane}>
        <form className={styles.form} onSubmit={handleSubmit}>
          <h3 style={{ margin: 0 }}>Sign in</h3>
          <p className={styles.hint}>Use the bootstrap administrator created on first run.</p>
          <hr className="hr" style={{ margin: '4px 0' }} />

          <div className="field">
            <label htmlFor="email">Email</label>
            <input
              id="email"
              type="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              autoFocus
              required
            />
          </div>

          <div className="field">
            <label htmlFor="password">Password</label>
            <input
              id="password"
              type="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
            />
          </div>

          <button type="submit" className="btn btn-primary" disabled={isSubmitting} style={{ marginTop: 4 }}>
            {isSubmitting ? 'Signing in...' : 'Sign in'}
          </button>

          {error !== null && <StatusMessage kind="error" message={getErrorMessage(error)} />}
        </form>
      </div>
    </div>
  )
}
