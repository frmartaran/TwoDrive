import { Injectable } from '@angular/core';
import { LoginService } from 'src/app/services/login.service';
import { map, take } from 'rxjs/operators';
import { 
  ActivatedRouteSnapshot, 
  CanActivate, 
  Router, 
  RouterStateSnapshot } from '@angular/router';
import { Observable, pipe } from 'rxjs';

@Injectable()
export class UserLoggedIn implements CanActivate {

  constructor(public loginService: LoginService, public router: Router) {}

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> {
    return this.loginService.getIsAuthenticated
      .pipe(
        take(1),
        map((isLoggedIn: boolean) => {
          if (isLoggedIn){
            this.router.navigate(['/home-page']);
            return false;
          }
          return true;
        })
      )
  }
}