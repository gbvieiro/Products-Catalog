interface StatusMessageProps {
  kind: 'loading' | 'error' | 'empty'
  message?: string
}

/** Componente pequeno e reutilizavel pros 3 estados mais comuns de uma tela que busca dados. */
export function StatusMessage({ kind, message }: StatusMessageProps) {
  if (kind === 'loading') return <p>Loading...</p>
  if (kind === 'error') return <p style={{ color: '#b91c1c' }}>{message ?? 'Something went wrong.'}</p>
  return <p style={{ color: '#666' }}>{message ?? 'Nothing here yet.'}</p>
}
