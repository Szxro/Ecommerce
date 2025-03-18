import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { NotFoundComponent } from './components/not-found/not-found.component';
import { SpinnerComponent } from './components/spinner/spinner.component';
import { ErrorMessageDirective } from './directives/error-message.directive';



@NgModule({
  declarations: [
    NotFoundComponent,
    SpinnerComponent,
    ErrorMessageDirective
  ],
  exports:[
    NotFoundComponent,
    SpinnerComponent,
    ErrorMessageDirective
  ],
  imports: [
    CommonModule
  ]
})
export class SharedModule { }
