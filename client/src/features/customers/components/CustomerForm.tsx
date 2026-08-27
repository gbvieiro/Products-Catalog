import { type FormEvent, useEffect, useState } from 'react'
import { useCreateCustomer } from '../hooks/useCreateCustomer'
import { useUpdateCustomer } from '../hooks/useUpdateCustomer'
import { getErrorMessage } from '../../../shared/api/httpClient'
import { StatusMessage } from '../../../shared/ui/StatusMessage'
import { Modal } from '../../../shared/ui/Modal'
import type { Customer } from '../types'

interface CustomerFormProps {
  /** Quando presente, o form edita este cliente em vez de criar um novo. */
  editingCustomer?: Customer | null
  onClose: () => void
}

/** Um unico form (em Modal) para criar e editar clientes (troca de modo via `editingCustomer`). */
export function CustomerForm({ editingCustomer, onClose }: CustomerFormProps) {
  const createCustomer = useCreateCustomer()
  const updateCustomer = useUpdateCustomer()

  const [name, setName] = useState('')
  const [email, setEmail] = useState('')

  const isEditing = Boolean(editingCustomer)
  const mutation = isEditing ? updateCustomer : createCustomer

  useEffect(() => {
    setName(editingCustomer?.name ?? '')
    setEmail(editingCustomer?.email ?? '')
  }, [editingCustomer])

  const handleSubmit = (event: FormEvent) => {
    event.preventDefault()

    if (isEditing && editingCustomer) {
      updateCustomer.mutate(
        { id: editingCustomer.id, input: { name, email } },
        { onSuccess: () => onClose() },
      )
      return
    }

    createCustomer.mutate({ name, email }, { onSuccess: () => onClose() })
  }

  return (
    <Modal title={isEditing ? 'Edit customer' : 'New customer'} onClose={onClose}>
      <form onSubmit={handleSubmit} style={{ display: 'flex', flexDirection: 'column', gap: 16 }}>
        <div className="dialog-body">
          <div className="field">
            <label htmlFor="name">Name</label>
            <input id="name" value={name} onChange={(e) => setName(e.target.value)} required maxLength={100} />
          </div>

          <div className="field">
            <label htmlFor="email">Email</label>
            <input id="email" type="email" value={email} onChange={(e) => setEmail(e.target.value)} required />
          </div>

          {mutation.isError && <StatusMessage kind="error" message={getErrorMessage(mutation.error)} />}
        </div>

        <div className="dialog-actions">
          <button type="button" className="btn btn-secondary" onClick={onClose}>Cancel</button>
          <button type="submit" className="btn btn-primary" disabled={mutation.isPending}>
            {isEditing
              ? mutation.isPending ? 'Saving...' : 'Save changes'
              : mutation.isPending ? 'Creating...' : 'Create customer'}
          </button>
        </div>
      </form>
    </Modal>
  )
}
