export type ErrorResponse = {
  type:    string;
  title:   string;
  status:  number;
  detail:  string;
  traceId: string;
  errors?: Record<string,Errors[]>
}

type Errors = {
  errorCode: string;
  description: string;
}
