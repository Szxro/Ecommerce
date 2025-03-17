import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, Router, RouterStateSnapshot } from '@angular/router';
import { AuthService } from '../services/auth.service';

export function isAuthenticatedGuard(route: ActivatedRouteSnapshot,state: RouterStateSnapshot){
  const router = inject(Router);
  const authService = inject(AuthService);

  const isAuthenticated = authService.isAuthenticated();

  if(!isAuthenticated){
      return router.navigateByUrl('/auth/login');
  }

  return true;
}
