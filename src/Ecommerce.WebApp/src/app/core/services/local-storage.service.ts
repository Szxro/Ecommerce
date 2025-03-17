import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LocalStorageService {
  getItemByKey<T = null>(key:string):T | null{
    const item = localStorage.getItem(key);
    if(!item) return null;

    try{
      const value = JSON.parse(item) as T;
      return value;
    }catch(_){
      return null;
    }
  }

  setItemByKey<T>(key:string, content:T):void{
    const value = JSON.stringify(content);

    localStorage.setItem(key,value);
  }

  removeItemByKey(key:string):void{
    localStorage.removeItem(key);
  }

  purge():void{
    localStorage.clear();
  }
}
