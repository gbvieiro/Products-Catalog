// Espelha ProductsCatalog.Application.Common.Models.PagedResult<T>
export interface PagedResult<T> {
  items: T[]
  totalCount: number
  skip: number
  take: number
}
