import type { User } from '../types'
import { useDeleteUser } from '../hooks/useDeleteUser'
import { ROLE_LABELS, Role } from '../../../shared/types/role'

interface UsersTableProps {
  users: User[]
  onEdit: (user: User) => void
}

export function UsersTable({ users, onEdit }: UsersTableProps) {
  const deleteUser = useDeleteUser()

  return (
    <table className="table">
      <thead>
        <tr>
          <th>Email</th>
          <th style={{ width: 200 }}>Role</th>
          <th style={{ width: 140 }}>Created</th>
          <th style={{ width: 170 }} />
        </tr>
      </thead>
      <tbody>
        {users.map((user) => (
          <tr key={user.id}>
            <td style={{ fontWeight: 600 }}>{user.email}</td>
            <td>
              <span className={user.role === Role.Administrator ? 'tag tag-accent' : 'tag tag-neutral'}>
                {ROLE_LABELS[user.role]}
              </span>
            </td>
            <td style={{ fontSize: 13, opacity: 0.7 }}>{new Date(user.createdAt).toLocaleDateString()}</td>
            <td style={{ textAlign: 'right', display: 'flex', gap: 4, justifyContent: 'flex-end' }}>
              <button type="button" className="btn btn-ghost rowact" onClick={() => onEdit(user)}>
                Edit
              </button>
              <button
                type="button"
                className="btn btn-ghost rowact"
                disabled={deleteUser.isPending}
                onClick={() => {
                  if (confirm(`Delete user ${user.email}?`)) {
                    deleteUser.mutate(user.id)
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
