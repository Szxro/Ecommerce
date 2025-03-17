export type SuccessResponse<T> = {
  detail: string;
  type:   string;
  status: number;
  data: T
};
