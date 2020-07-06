import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { AuthService } from '../_services/auth.service';

@Injectable({
  providedIn: 'root',
})
// Used to guard different routes from unauthorised access
export class AuthGuard implements CanActivate {
  constructor(
    private authService: AuthService,
    private alertify: AlertifyService,
    private router: Router
  ) {}

  canActivate(): boolean {
    // Check if user is logged in
    if (this.authService.loggedIn()) {
      return true;
    }
    // Gives an error if not and redirects to home page
    this.alertify.error('You shall not pass!');
    this.router.navigate(['/home']);
    return false;
  }
}
