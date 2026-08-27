import type { ReactNode } from 'react'
import { PlusIcon } from './icons'
import styles from './PageHeader.module.css'

interface PageHeaderProps {
  title: string
  /** Botao de acao primaria no canto direito (ex: "New order"). Sem onClick, nao renderiza nada. */
  action?: { label: string; onClick: () => void }
  children?: ReactNode
}

/** Cabecalho fixo do topo de cada pagina (titulo + acao primaria), mesmo padrao em todas as telas de CRUD. */
export function PageHeader({ title, action, children }: PageHeaderProps) {
  return (
    <header className={styles.header}>
      <h4 className={styles.title}>{title}</h4>
      {children}
      {action && (
        <button type="button" className="btn btn-primary" onClick={action.onClick}>
          <PlusIcon style={{ width: 15, height: 15 }} />
          <span>{action.label}</span>
        </button>
      )}
    </header>
  )
}
