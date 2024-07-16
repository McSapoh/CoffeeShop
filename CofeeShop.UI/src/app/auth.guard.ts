import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable, map } from 'rxjs';
import { AuthService } from './services/auth/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
      if (this.authService.isLoggedIn()) {
        // user is logged in, allow navigation
        return true;
      } else {
        return this.authService.refreshToken().pipe(
          map((result: boolean) => {
            console.log(result);
            if (!result) {
              this.router.navigate(['/Login'], { queryParams: { 'redirectURL': state.url } });
              return false;
            }
            return true;
          })
        );
      }
  }
}
