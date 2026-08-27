import { createBrowserRouter } from 'react-router-dom'
import { Layout } from '../shared/ui/Layout'
import { ProtectedRoute } from '../shared/auth/ProtectedRoute'
import { Role } from '../shared/types/role'
import { LoginPage } from '../features/auth/pages/LoginPage'
import { OverviewPage } from '../features/home/pages/OverviewPage'
import { BooksPage } from '../features/books/pages/BooksPage'
import { CustomersPage } from '../features/customers/pages/CustomersPage'
import { OrdersPage } from '../features/orders/pages/OrdersPage'
import { StocksPage } from '../features/stocks/pages/StocksPage'
import { UsersPage } from '../features/users/pages/UsersPage'

export const router = createBrowserRouter([
  { path: '/login', element: <LoginPage /> },
  {
    // Exige apenas estar autenticado - Administrator e Seller passam.
    element: <ProtectedRoute />,
    children: [
      {
        path: '/',
        element: <Layout />,
        children: [
          { index: true, element: <OverviewPage /> },
          // Orders: unico menu que o Seller enxerga (ver Layout) - criar/ver pedidos e sua unica permissao.
          { path: 'orders', element: <OrdersPage /> },
          {
            // Cadastros administrativos: exclusivos do Administrator, tanto no
            // menu (ver Layout) quanto aqui (acesso direto pela URL tambem e barrado).
            element: <ProtectedRoute allowedRoles={[Role.Administrator]} />,
            children: [
              { path: 'books', element: <BooksPage /> },
              { path: 'stocks', element: <StocksPage /> },
              { path: 'customers', element: <CustomersPage /> },
              { path: 'users', element: <UsersPage /> },
            ],
          },
        ],
      },
    ],
  },
])
