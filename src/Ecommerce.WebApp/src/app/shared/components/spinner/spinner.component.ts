import { Component } from '@angular/core';
import { SpinnerService } from '../../../core/services/spinner.service';

@Component({
  selector: 'shared-spinner',
  template:`
    <div class="overlay" *ngIf="isLoading$ | async">
      <div class="dual-ring"></div>
    </div>
  `,
  styleUrl: './spinner.component.scss'
})
export class SpinnerComponent {
  public isLoading$ = this._spinnerService.isLoading$;

  constructor(
    private readonly _spinnerService:SpinnerService
  ){}
}
