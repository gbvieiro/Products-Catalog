// Espelha ProductsCatalog.Application.Features.Auth.* no backend.
import type { Role } from '../../shared/types/role'

export interface LoginInput {
  email: string
  password: string
}

export interface AuthenticatedUser {
  id: string
  email: string
  role: Role
}

export interface LoginResult {
  token: string
  expiresAtUtc: string
  user: AuthenticatedUser
}
