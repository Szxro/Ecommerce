import { AbstractControl, ValidationErrors } from "@angular/forms";

export class CustomValidators{
  static passwordMustMatch(control:AbstractControl):ValidationErrors | null{
      const password = control.get('password');
      const confirmPassword = control.get('confirmPassword');

      return password && confirmPassword && confirmPassword.value !== password.value
          ? { passwordMatchError : true }
          : null;
  }

  static passwordStrength(control:AbstractControl):ValidationErrors | null{
      const value = control.value || '';

      const hasUpperCase = /[A-Z]/.test(value);
      const hasLowerCase = /[a-z]/.test(value);
      const hasNumber = /[0-9]/.test(value);
      const hasSpecialChar = /[\W_]/.test(value);

      const isValid = hasUpperCase && hasLowerCase && hasNumber && hasSpecialChar;

      return isValid ? null : { weakPassword: true }
  }

  static usernameFormat(control:AbstractControl):ValidationErrors | null{
      const value = control.value || '';

      const isValid = /^[a-zA-Z0-9_]+$/.test(value);

      return isValid ? null : { invalidUsername: true }
  }
}
