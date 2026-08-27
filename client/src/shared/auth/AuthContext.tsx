import { createContext, useContext, useEffect, useMemo, useState, type PropsWithChildren } from 'react'
import { useQueryClient } from '@tanstack/react-query'
import { authApi } from '../../features/auth/api/authApi'
import type { AuthenticatedUser, LoginInput } from '../../features/auth/types'
import { setAuthToken, setUnauthorizedHandler } from '../api/httpClient'
import type { Role } from '../types/role'

const STORAGE_KEY = 'products-catalog.auth'

interface StoredSession {
  token: string
  expiresAtUtc: string
  user: AuthenticatedUser
}

function readStoredSession(): StoredSession | null {
  try {
    const raw = localStorage.getItem(STORAGE_KEY)
    if (!raw) return null

    const session = JSON.parse(raw) as StoredSession
    if (new Date(session.expiresAtUtc).getTime() <= Date.now()) {
      // Token expirado - nao ha refresh token nesta versao (ver README), entao
      // a sessao simplesmente termina e o usuario precisa logar de novo.
      localStorage.removeItem(STORAGE_KEY)
      return null
    }
    return session
  } catch {
    return null
  }
}

interface AuthContextValue {
  user: AuthenticatedUser | null
  isAuthenticated: boolean
  /** false ate a primeira leitura de localStorage terminar - evita "piscar" a tela de login antes da hora. */
  isInitializing: boolean
  login: (input: LoginInput) => Promise<void>
  logout: () => void
  hasRole: (...roles: Role[]) => boolean
}

const AuthContext = createContext<AuthContextValue | null>(null)

export function AuthProvider({ children }: PropsWithChildren) {
  const queryClient = useQueryClient()
  const [session, setSession] = useState<StoredSession | null>(null)
  const [isInitializing, setIsInitializing] = useState(true)

  useEffect(() => {
    const stored = readStoredSession()
    if (stored) {
      setAuthToken(stored.token)
      setSession(stored)
    }
    setIsInitializing(false)
  }, [])

  useEffect(() => {
    setUnauthorizedHandler(() => logout())
    return () => setUnauthorizedHandler(null)
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [])

  const login = async (input: LoginInput) => {
    const result = await authApi.login(input)
    const newSession: StoredSession = {
      token: result.token,
      expiresAtUtc: result.expiresAtUtc,
      user: result.user,
    }

    localStorage.setItem(STORAGE_KEY, JSON.stringify(newSession))
    setAuthToken(newSession.token)
    setSession(newSession)
  }

  const logout = () => {
    // Fire-and-forget: logout e stateless no servidor (ver authApi.logout),
    // entao nao ha motivo pra bloquear o logout local esperando essa chamada
    // (e ela pode nem ser possivel se o token ja expirou).
    authApi.logout().catch(() => {})

    localStorage.removeItem(STORAGE_KEY)
    setAuthToken(null)
    setSession(null)
    queryClient.clear()
  }

  const value = useMemo<AuthContextValue>(
    () => ({
      user: session?.user ?? null,
      isAuthenticated: session !== null,
      isInitializing,
      login,
      logout,
      hasRole: (...roles: Role[]) => session !== null && roles.includes(session.user.role),
    }),
    [session, isInitializing],
  )

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>
}

export function useAuth(): AuthContextValue {
  const context = useContext(AuthContext)
  if (!context) {
    throw new Error('useAuth must be used within an AuthProvider')
  }
  return context
}
