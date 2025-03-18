import { InjectionToken } from "@angular/core";
import { DEFAULT_VALIDATION_MESSAGES } from "../constants/default-validation-messages";

export const VALIDATION_ERROR_TOKEN = new InjectionToken('Validation Errors Token',{
  providedIn:'root',
  factory: () => DEFAULT_VALIDATION_MESSAGES
});
