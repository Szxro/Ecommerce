import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { spinnerInterceptor } from './core/interceptors/spinner.interceptor';
import { tokenInterceptor } from './core/interceptors/token.interceptor';
import { SharedModule } from './shared/shared.module';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserAnimationsModule,
    BrowserModule,
    AppRoutingModule,
    SharedModule,
    ToastrModule.forRoot({
      timeOut:3000,
      preventDuplicates: true
    }),
  ],
  providers: [
    provideHttpClient(
      withInterceptors([ spinnerInterceptor, tokenInterceptor])
    )
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
