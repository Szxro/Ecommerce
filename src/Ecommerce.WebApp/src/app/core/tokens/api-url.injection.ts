import { InjectionToken } from "@angular/core";
import { environment } from "../../../environments/environment.development";

export const API_URL_TOKEN = new InjectionToken('Base api url',{
  providedIn:'root',
  factory: () => environment.api_url
});
