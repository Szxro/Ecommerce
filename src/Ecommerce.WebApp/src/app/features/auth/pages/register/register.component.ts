import { Component, OnDestroy } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { catchError, EMPTY, Subject, takeUntil } from 'rxjs';
import { ErrorResponse } from '../../../../core/models/responses/error-response.model';
import { AuthService } from '../../../../core/services/auth.service';
import { ToastService } from '../../../../core/services/toast.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent implements OnDestroy {
  readonly registerForm = this._formBuilder.nonNullable.group({
    firstName:['',[Validators.required,Validators.minLength(1)]],
    lastName: ['',[Validators.required,Validators.minLength(1)]],
    username: ['',[Validators.required,Validators.minLength(3),Validators.maxLength(20)]],
    email:    ['',[Validators.required,Validators.email]],
    password: ['',[Validators.required,Validators.minLength(6)]],
    confirmPassword: ['',Validators.required]
  });

  private readonly _onDestroy$ = new Subject<void>();

  constructor(
    private readonly _formBuilder:FormBuilder,
    private readonly _authService: AuthService,
    private readonly _toastService:ToastService,
    private readonly _router:Router
  ){}

  ngOnDestroy(): void {
    this._onDestroy$.next();
    this._onDestroy$.complete();
  }

  onSubmit():void{
    const { confirmPassword, ...registerRequest } = this.registerForm.getRawValue();

    this._authService.register(registerRequest)
    .pipe(
      takeUntil(this._onDestroy$),
      catchError((err:ProgressEvent | ErrorResponse) =>{
        this._toastService.error(err);

        return EMPTY;
      })
    )
    .subscribe({
      next: () =>{
        this._toastService.sucess({ message:"You successfully created an account on PulseHub!!" });
        this._router.navigateByUrl('/auth/login');
      },
      complete: () => this.registerForm.reset()
    });
  }
}
