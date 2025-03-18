import { AfterViewInit, DestroyRef, Directive, ElementRef, Inject, Input } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { AbstractControl, FormGroup, ValidationErrors } from '@angular/forms';
import { ErrorHandler } from '../../core/constants/default-validation-messages';
import { VALIDATION_ERROR_TOKEN } from '../../core/tokens/validation-error.injection';

type Options = {
  type:"field" | "global",
  errors:ValidationErrors
}

@Directive({
   selector: '[errorMessage]'
  })
export class ErrorMessageDirective implements AfterViewInit {
  @Input({ required: true }) target!: AbstractControl | FormGroup;

  constructor(
    private readonly _elementRef:ElementRef<HTMLSpanElement>,
    private readonly _destroyRef:DestroyRef,
    @Inject(VALIDATION_ERROR_TOKEN) private _errors: ErrorHandler
  ) { }

  ngAfterViewInit(): void {
    this.target.statusChanges
    .pipe(
      takeUntilDestroyed(this._destroyRef)
    ).subscribe(() => this.setErrorMessage({
      type: this.target instanceof FormGroup ? "global" : "field"
    }));
  }

  private getErrorMessage({type, errors}:Options):string{
    const [errorKey, errorValue] = Object.entries(errors)[0];

    const message = this._errors[type]?.[errorKey]
      ? this._errors[type][errorKey](errorValue)
      : "Unknown message";

    return message;
  }

  private setErrorMessage({ type }:Omit<Options,'errors'>){
    if(this.target.errors && this.target.dirty){
      const message = this.getErrorMessage({ type,errors: this.target.errors });
      this._elementRef.nativeElement.textContent = message;
      return;
    }
    this._elementRef.nativeElement.textContent = "";
  }
}
