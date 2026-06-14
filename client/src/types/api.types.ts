export interface ApiResult<T> {
  success: boolean
  data?: T
  errors?: Record<string, string[]>
}

export interface PaginatedResult<T> {
  items: T[]
  totalCount: number
  pageNumber: number
  pageSize: number
}
