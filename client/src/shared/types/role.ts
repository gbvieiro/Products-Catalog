// Espelha ProductsCatalog.Domain.Enums.ERole no backend. Sem
// JsonStringEnumConverter configurado na Api, o enum trafega como numero no
// JSON (nao como "Administrator"/"Seller") - por isso os valores numericos
// abaixo precisam bater exatamente com o enum do backend.
//
// Usamos um objeto const (em vez de `enum`) porque o tsconfig deste projeto
// tem `erasableSyntaxOnly: true` - `enum` gera codigo em tempo de execucao e
// nao e uma construcao "erasable", entao o build falha com TS1294. Este
// padrao (const object + type derivado) e o substituto recomendado.
export const Role = {
  Administrator: 1,
  Seller: 2,
} as const

export type Role = (typeof Role)[keyof typeof Role]

export const ROLE_LABELS: Record<Role, string> = {
  [Role.Administrator]: 'Administrator',
  [Role.Seller]: 'Seller',
}

export const ROLE_OPTIONS: Array<{ value: Role; label: string }> = [
  { value: Role.Administrator, label: ROLE_LABELS[Role.Administrator] },
  { value: Role.Seller, label: ROLE_LABELS[Role.Seller] },
]
