import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ErrorResponse } from '../models/responses/error-response.model';

type ToastMessage = {
  title?:string;
  message:string;
}

@Injectable({
  providedIn: 'root'
})
export class ToastService {
  constructor(
    private readonly _toastr:ToastrService
  ) { }

  error(error:ToastMessage):void;
  error(error:ProgressEvent | ErrorResponse):void;
  error(error:unknown):void{
      switch(true){
          case error instanceof ProgressEvent:
              this.showError({message: "Can't connect to the server, try again later"});
              break;
          case this.isErrorResponse(error):
              this.handleErrorResponse(error);
              break;
          case this.isToastMessage(error):
              this.showError({message: error.message,title: error.title});
              break;
          default:
              this.showError({message: 'An unexpected error occurred'});
              break;
      }
  }

  sucess({message, title = "Success!!"}:ToastMessage):void{
    this._toastr.success(message,title,{
        progressBar:true,
        closeButton:true,
        progressAnimation:'decreasing'
    });
  }

  // Helpers

  private handleErrorResponse(error:ErrorResponse):void{
    if(error.errors !== undefined){
      for(const errors of Object.values(error.errors)){
          for(const { description } of errors){
              this.showError({message: description});
          }
      }
      return;
    }
    this.showError({message: error.detail});
}

  private showError({ message, title = "Error" }:ToastMessage){
    this._toastr.error(message,title,{
        progressBar:true,
        closeButton:true,
        progressAnimation:'decreasing'
    });
  }

  private isErrorResponse(error:unknown):error is ErrorResponse{
    return error != null && (error as ErrorResponse).detail !== undefined;
  }

  private isToastMessage(error:unknown):error is ToastMessage{
      return error != null && (error as ToastMessage).message !== undefined;
  }
}
