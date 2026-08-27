import { useState } from 'react'
import { NavLink, Outlet } from 'react-router-dom'
import { useAuth } from '../auth/AuthContext'
import { Role, ROLE_LABELS } from '../types/role'
import { BooksIcon, CustomersIcon, HomeIcon, LogoutIcon, MenuIcon, OrdersIcon, StocksIcon, UsersIcon } from './icons'
import styles from './Layout.module.css'

const NAV_ITEMS = [
  { to: '/', label: 'Overview', icon: HomeIcon, end: true },
  { to: '/orders', label: 'Orders', icon: OrdersIcon },
]

const ADMIN_NAV_ITEMS = [
  { to: '/books', label: 'Books', icon: BooksIcon },
  { to: '/stocks', label: 'Stocks', icon: StocksIcon },
  { to: '/customers', label: 'Customers', icon: CustomersIcon },
  { to: '/users', label: 'Users', icon: UsersIcon },
]

function initialsOf(email: string): string {
  const local = email.split('@')[0] ?? email
  const parts = local.split(/[.\-_]/).filter(Boolean)
  const chars = parts.length > 1 ? [parts[0][0], parts[1][0]] : [local[0], local[1] ?? '']
  return chars.join('').toUpperCase()
}

export function Layout() {
  const { user, hasRole, logout } = useAuth()
  const [collapsed, setCollapsed] = useState(false)
  const isAdmin = hasRole(Role.Administrator)

  return (
    <div className={styles.shell}>
      <aside className={`${styles.sidebar} ${collapsed ? styles.sidebarCollapsed : ''}`}>
        <div className={styles.sidebarHead}>
          <button
            type="button"
            className="btn btn-icon"
            onClick={() => setCollapsed((value) => !value)}
            title="Toggle menu"
            style={{ color: 'inherit', flex: 'none' }}
          >
            <MenuIcon style={{ width: 18, height: 18 }} />
          </button>
          {!collapsed && <span className={styles.brand}>PRODUCTS CATALOG</span>}
        </div>

        <div className={styles.divider} />

        <nav className={styles.nav}>
          {NAV_ITEMS.map((item) => (
            <NavLink
              key={item.to}
              to={item.to}
              end={item.end}
              title={item.label}
              className={({ isActive }) => `${styles.navItem} ${isActive ? styles.navItemActive : ''}`}
            >
              <item.icon style={{ width: 18, height: 18, flex: 'none' }} />
              {!collapsed && <span>{item.label}</span>}
            </NavLink>
          ))}

          {isAdmin && (
            <>
              {!collapsed && <div className={styles.navSectionLabel}>Administration</div>}
              {ADMIN_NAV_ITEMS.map((item) => (
                <NavLink
                  key={item.to}
                  to={item.to}
                  title={item.label}
                  className={({ isActive }) => `${styles.navItem} ${isActive ? styles.navItemActive : ''}`}
                >
                  <item.icon style={{ width: 18, height: 18, flex: 'none' }} />
                  {!collapsed && <span>{item.label}</span>}
                </NavLink>
              ))}
            </>
          )}
        </nav>

        <div className={styles.divider} />

        {user && (
          <div className={styles.userRow}>
            <div className={styles.userAvatar}>{initialsOf(user.email)}</div>
            {!collapsed && (
              <div className={styles.userInfo}>
                <div className={styles.userEmail}>{user.email}</div>
                <div className={styles.userRole}>{ROLE_LABELS[user.role]}</div>
              </div>
            )}
            {!collapsed && (
              <button
                type="button"
                className="btn btn-icon"
                onClick={logout}
                title="Log out"
                style={{ color: 'inherit', flex: 'none' }}
              >
                <LogoutIcon style={{ width: 17, height: 17 }} />
              </button>
            )}
          </div>
        )}
      </aside>

      <main className={styles.main}>
        <Outlet />
      </main>
    </div>
  )
}
