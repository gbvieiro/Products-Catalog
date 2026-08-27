import { type FormEvent, useEffect, useState } from 'react'
import { useCreateUser } from '../hooks/useCreateUser'
import { useUpdateUser } from '../hooks/useUpdateUser'
import { getErrorMessage } from '../../../shared/api/httpClient'
import { StatusMessage } from '../../../shared/ui/StatusMessage'
import { Modal } from '../../../shared/ui/Modal'
import { Role, ROLE_OPTIONS } from '../../../shared/types/role'
import type { User } from '../types'

interface UserFormProps {
  /** Quando presente, o form edita este usuario em vez de criar um novo. */
  editingUser?: User | null
  onClose: () => void
}

/**
 * Um unico form (em Modal) para criar e editar usuarios (troca de modo via
 * `editingUser`). A senha so aparece no modo de criacao - trocar senha e um
 * fluxo a parte (normalmente exigindo a senha atual como confirmacao), fora
 * do escopo deste CRUD. Ver UpdateUserCommand no backend: so Email e Role
 * sao editaveis.
 */
export function UserForm({ editingUser, onClose }: UserFormProps) {
  const createUser = useCreateUser()
  const updateUser = useUpdateUser()

  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [role, setRole] = useState<Role>(Role.Seller)

  const isEditing = Boolean(editingUser)
  const mutation = isEditing ? updateUser : createUser

  useEffect(() => {
    setEmail(editingUser?.email ?? '')
    setPassword('')
    setRole(editingUser?.role ?? Role.Seller)
  }, [editingUser])

  const handleSubmit = (event: FormEvent) => {
    event.preventDefault()

    if (isEditing && editingUser) {
      updateUser.mutate(
        { id: editingUser.id, input: { email, role } },
        { onSuccess: () => onClose() },
      )
      return
    }

    createUser.mutate({ email, password, role }, { onSuccess: () => onClose() })
  }

  return (
    <Modal title={isEditing ? 'Edit user' : 'New user'} onClose={onClose}>
      <form onSubmit={handleSubmit} style={{ display: 'flex', flexDirection: 'column', gap: 16 }}>
        <div className="dialog-body">
          <div className="field">
            <label htmlFor="email">Email</label>
            <input id="email" type="email" value={email} onChange={(e) => setEmail(e.target.value)} required />
          </div>

          {!isEditing && (
            <div className="field">
              <label htmlFor="password">Password</label>
              <input
                id="password"
                type="password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                minLength={6}
                required
              />
            </div>
          )}

          <div className="field">
            <label>Role</label>
            <div className="seg">
              {ROLE_OPTIONS.map((option) => (
                <label key={option.value} className="seg-opt">
                  <input
                    type="radio"
                    name="role"
                    checked={role === option.value}
                    onChange={() => setRole(option.value)}
                  />
                  <span>{option.label}</span>
                </label>
              ))}
            </div>
          </div>

          {mutation.isError && <StatusMessage kind="error" message={getErrorMessage(mutation.error)} />}
        </div>

        <div className="dialog-actions">
          <button type="button" className="btn btn-secondary" onClick={onClose}>Cancel</button>
          <button type="submit" className="btn btn-primary" disabled={mutation.isPending}>
            {isEditing
              ? mutation.isPending ? 'Saving...' : 'Save changes'
              : mutation.isPending ? 'Creating...' : 'Create user'}
          </button>
        </div>
      </form>
    </Modal>
  )
}
