import type { Customer } from '../types'
import { useDeleteCustomer } from '../hooks/useDeleteCustomer'

interface CustomersTableProps {
  customers: Customer[]
  onEdit: (customer: Customer) => void
}

export function CustomersTable({ customers, onEdit }: CustomersTableProps) {
  const deleteCustomer = useDeleteCustomer()

  return (
    <table className="table">
      <thead>
        <tr>
          <th>Name</th>
          <th style={{ width: 280 }}>Email</th>
          <th style={{ width: 140 }}>Created</th>
          <th style={{ width: 170 }} />
        </tr>
      </thead>
      <tbody>
        {customers.map((customer) => (
          <tr key={customer.id}>
            <td style={{ fontWeight: 600 }}>{customer.name}</td>
            <td style={{ fontSize: 13 }}>{customer.email}</td>
            <td style={{ fontSize: 13, opacity: 0.7 }}>{new Date(customer.createdAt).toLocaleDateString()}</td>
            <td style={{ textAlign: 'right', display: 'flex', gap: 4, justifyContent: 'flex-end' }}>
              <button type="button" className="btn btn-ghost rowact" onClick={() => onEdit(customer)}>
                Edit
              </button>
              <button
                type="button"
                className="btn btn-ghost rowact"
                disabled={deleteCustomer.isPending}
                onClick={() => {
                  if (confirm(`Delete customer ${customer.name}?`)) {
                    deleteCustomer.mutate(customer.id)
                  }
                }}
              >
                Delete
              </button>
            </td>
          </tr>
        ))}
      </tbody>
    </table>
  )
}
