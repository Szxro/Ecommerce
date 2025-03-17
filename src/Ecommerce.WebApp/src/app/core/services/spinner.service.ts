import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SpinnerService {
  private readonly isLoadingSubject$ = new BehaviorSubject<boolean>(false);
  // To handle multiple http requests
  private readonly requestMap:Map<string,boolean> = new Map();

  public isLoading$ = this.isLoadingSubject$.asObservable();

  updateLoadingStatus(url:string,status:boolean){
      if(!url){
          throw Error('The request URL must be provided');
      }

      if(status){
          this.requestMap.set(url,status);
      }else{
          this.requestMap.delete(url);
      }

      // Updating the status base on the size of the map
      this.isLoadingSubject$.next(this.requestMap.size > 0);
  }
}
