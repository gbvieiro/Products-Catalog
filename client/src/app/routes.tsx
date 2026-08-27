import { createBrowserRouter, Navigate } from 'react-router-dom'
import { Layout } from '../shared/ui/Layout'
import { BooksPage } from '../features/books/pages/BooksPage'
import { OrdersPage } from '../features/orders/pages/OrdersPage'
import { StocksPage } from '../features/stocks/pages/StocksPage'
import { UsersPage } from '../features/users/pages/UsersPage'

export const router = createBrowserRouter([
  {
    path: '/',
    element: <Layout />,
    children: [
      { index: true, element: <Navigate to="/orders" replace /> },
      { path: 'orders', element: <OrdersPage /> },
      { path: 'books', element: <BooksPage /> },
      { path: 'stocks', element: <StocksPage /> },
      { path: 'users', element: <UsersPage /> },
    ],
  },
])
