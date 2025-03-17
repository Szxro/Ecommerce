import { HttpClient, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { catchError, Observable, tap, throwError } from 'rxjs';
import { LoginRequest } from '../models/requests/login.request';
import { RegenerateToken } from '../models/requests/regenerate-token.request';
import { RegisterRequest } from '../models/requests/register.request';
import { SuccessResponse } from '../models/responses/success-response.model';
import { TokenResponse } from '../models/responses/token-response.model';
import { API_URL_TOKEN } from '../tokens/api-url.injection';
import { LocalStorageService } from './local-storage.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(
    private readonly _localStorage:LocalStorageService,
    private readonly _http:HttpClient,
    @Inject(API_URL_TOKEN) private readonly _apiUrl:string
  ) { }

  login(credentials:LoginRequest): Observable<SuccessResponse<TokenResponse>>{
    return this._http.post<SuccessResponse<TokenResponse>>(`${this._apiUrl}/user/login-user`,credentials)
      .pipe(
        tap(({data})=> this._localStorage.setItemByKey("token",data)),
        catchError((err:HttpErrorResponse) => throwError(() => err.error))
      )
  };

  register(registerRequest:RegisterRequest): Observable<HttpResponse<Object>>{
    return this._http.post<HttpResponse<Object>>(
      `${this._apiUrl}/user/register-user`,registerRequest,{ observe: 'response'}
    ).pipe(
      catchError((err:HttpErrorResponse) => throwError(() => err.error))
    );
  }

  regenerateToken(request:RegenerateToken): Observable<SuccessResponse<TokenResponse>>{
    return this._http.post<SuccessResponse<TokenResponse>>(`${this._apiUrl}/token/regenerate`,request).pipe(
      tap({
          next: ({data}) => this._localStorage.setItemByKey('token',data)
      }),
      catchError((err:HttpErrorResponse) => throwError(() => err.error))
    );
  }

  logout():void{
    this._localStorage.purge();
  }

  isAuthenticated():boolean{
    const token = this._localStorage.getItemByKey('token');
    return !!token;
  }
}
