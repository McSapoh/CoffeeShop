export interface Pagination {
  CurrentPage: number;
  PageSize: number;
  TotalCount: number;
  TotalPages: number;
}

export class PaginatedResult<T>{
  result!: T;
  pagination!: Pagination;
}