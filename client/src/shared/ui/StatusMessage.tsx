interface StatusMessageProps {
  kind: 'loading' | 'error' | 'empty'
  message?: string
}

/** Componente pequeno e reutilizavel pros 3 estados mais comuns de uma tela que busca dados. */
export function StatusMessage({ kind, message }: StatusMessageProps) {
  if (kind === 'loading') return <p className="text-muted" style={{ fontSize: 13 }}>Loading...</p>
  if (kind === 'error') return <p style={{ color: '#c92b18', fontSize: 13 }}>{message ?? 'Something went wrong.'}</p>
  return <div className="empty-state">{message ?? 'Nothing here yet.'}</div>
}
