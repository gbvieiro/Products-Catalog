import type { Role } from '../../shared/types/role'

// Espelha ProductsCatalog.Application.Features.Users.* no backend.
export interface User {
  id: string
  email: string
  role: Role
  createdAt: string
}

export interface CreateUserInput {
  email: string
  password: string
  role: Role
}

export interface UpdateUserInput {
  email: string
  role: Role
}
