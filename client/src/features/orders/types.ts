// Espelha ProductsCatalog.Application.Features.Orders.* no backend.
export const ORDER_STATUS = { 1: 'Created', 2: 'Confirmed', 3: 'Canceled' } as const

export interface OrderItem {
  bookId: string
  quantity: number
  unitPrice: number
  amount: number
}

export interface Order {
  id: string
  customerId: string
  status: keyof typeof ORDER_STATUS
  totalAmount: number
  createdAt: string
  items: OrderItem[]
}

export interface CreateOrderItemInput {
  bookId: string
  quantity: number
}

export interface CreateOrderInput {
  customerId: string
  items: CreateOrderItemInput[]
}

export interface CancelOrderResult {
  message: string
}
