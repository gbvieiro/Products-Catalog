// Espelha ProductsCatalog.Application.Features.Customers.* no backend.
export interface Customer {
  id: string
  name: string
  email: string
  createdAt: string
}

export interface CreateCustomerInput {
  name: string
  email: string
}

export interface UpdateCustomerInput {
  name: string
  email: string
}
