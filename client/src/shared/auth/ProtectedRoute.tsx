import { Navigate, Outlet, useLocation } from 'react-router-dom'
import { useAuth } from './AuthContext'
import type { Role } from '../types/role'

interface ProtectedRouteProps {
  /** Quando informado, restringe o acesso a estas roles (ex: telas de administracao). Sem isso, so exige estar logado. */
  allowedRoles?: Role[]
}

/**
 * Guarda de rota: redireciona para /login se nao autenticado (preservando a
 * rota de destino em `state.from` para redirecionar de volta apos o login), e
 * para / (Overview, acessivel a qualquer role) se autenticado mas sem role
 * suficiente (ex: Seller tentando abrir uma tela administrativa direto pela URL).
 */
export function ProtectedRoute({ allowedRoles }: ProtectedRouteProps) {
  const { isAuthenticated, isInitializing, hasRole } = useAuth()
  const location = useLocation()

  if (isInitializing) {
    return null
  }

  if (!isAuthenticated) {
    return <Navigate to="/login" replace state={{ from: location }} />
  }

  if (allowedRoles && !hasRole(...allowedRoles)) {
    return <Navigate to="/" replace />
  }

  return <Outlet />
}
