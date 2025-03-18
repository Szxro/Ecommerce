import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { ReactiveFormsModule } from '@angular/forms';
import { DEFAULT_VALIDATION_MESSAGES, ErrorHandler } from '../../core/constants/default-validation-messages';
import { VALIDATION_ERROR_TOKEN } from '../../core/tokens/validation-error.injection';
import { SharedModule } from '../../shared/shared.module';
import { AuthRoutingModule } from './auth-routing.module';
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';

const AUTH_VALIDATION_MESSAGES: ErrorHandler = {
  global:{
      ...DEFAULT_VALIDATION_MESSAGES.global,
      passwordMatchError: () => "Passwords must match.",
  },
  field:{
      ...DEFAULT_VALIDATION_MESSAGES.field,
      weakPassword: () => "The password at least need to have one uppercase, lowercase, number and special character",
      invalidUsername: () => "The username can only contain letters, numbers, and underscores",
      email:() => "The provide email need to be a valid email"
  }
}

@NgModule({
  declarations: [
    LoginComponent,
    RegisterComponent
  ],
  imports: [
    CommonModule,
    AuthRoutingModule,
    ReactiveFormsModule,
    SharedModule
  ],
  providers:[
    { provide:VALIDATION_ERROR_TOKEN, useValue:AUTH_VALIDATION_MESSAGES }
  ]
})
export class AuthModule { }
