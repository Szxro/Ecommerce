import { Component, DestroyRef } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { catchError, EMPTY } from 'rxjs';
import { ErrorResponse } from '../../../../core/models/responses/error-response.model';
import { AuthService } from '../../../../core/services/auth.service';
import { ToastService } from '../../../../core/services/toast.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  readonly loginForm = this._formBuilder.nonNullable.group({
    username: ['',[Validators.required,Validators.minLength(3),Validators.maxLength(20)]],
    password: ['',[Validators.required]]
  });

  constructor(
    private readonly _formBuilder:FormBuilder,
    private readonly _authService:AuthService,
    private readonly _destroyRef:DestroyRef,
    private readonly _router:Router,
    private readonly _toastService:ToastService
  ){}

  onSubmit():void{
    this._authService.login(
      this.loginForm.getRawValue()
    ).pipe(
      takeUntilDestroyed(this._destroyRef),
      catchError((err: ProgressEvent | ErrorResponse) =>{
        this._toastService.error(err);

        // Emit no values just complete the observable (complete: => complete signal)
        return EMPTY;
      })
    ).subscribe({
      next: () => this._router.navigate(['/home']),
      complete: () => this.loginForm.reset()
    });
  }
}
