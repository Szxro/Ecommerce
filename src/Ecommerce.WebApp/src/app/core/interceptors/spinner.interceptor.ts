import { HttpEvent, HttpHandlerFn, HttpRequest } from "@angular/common/http";
import { inject } from "@angular/core";
import { finalize, Observable } from "rxjs";
import { SpinnerService } from "../services/spinner.service";

export function spinnerInterceptor(req:HttpRequest<unknown>,next:HttpHandlerFn):Observable<HttpEvent<unknown>>{
  const spinner = inject(SpinnerService);

  // Start the spinner for the request
  spinner.updateLoadingStatus(req.url,true);

  return next(req).pipe(
    finalize(() =>{
      // Stop the spinner for the request even if an error happen
      spinner.updateLoadingStatus(req.url,false);
    })
  );
}
