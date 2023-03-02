export interface Result<T> {
  isSuccess: boolean;
  content: T;
  errors: string[];
}

export interface EmptyResult {
  isSuccess: boolean;
  errors: string[];
}