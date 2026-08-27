import { type MouseEvent, type PropsWithChildren, useEffect } from 'react'

interface ModalProps {
  title: string
  onClose: () => void
}

/**
 * Backdrop + dialog padrao do sistema de design (classes .dialog-backdrop/
 * .dialog/.dialog-title em index.css). Usado pelos forms de criacao/edicao
 * de cada feature, que ficam sob um botao "+" no header em vez de sempre
 * visiveis na pagina.
 */
export function Modal({ title, onClose, children }: PropsWithChildren<ModalProps>) {
  useEffect(() => {
    const onKeyDown = (event: KeyboardEvent) => {
      if (event.key === 'Escape') onClose()
    }
    document.addEventListener('keydown', onKeyDown)
    return () => document.removeEventListener('keydown', onKeyDown)
  }, [onClose])

  const stopPropagation = (event: MouseEvent) => event.stopPropagation()

  return (
    <div className="dialog-backdrop" onClick={onClose}>
      <div className="dialog" onClick={stopPropagation} role="dialog" aria-modal="true" aria-label={title}>
        <div className="dialog-title">{title}</div>
        {children}
      </div>
    </div>
  )
}
