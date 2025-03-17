import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { NotFoundComponent } from './components/not-found/not-found.component';



@NgModule({
  declarations: [
    NotFoundComponent
  ],
  exports:[
    NotFoundComponent
  ],
  imports: [
    CommonModule
  ]
})
export class SharedModule { }
