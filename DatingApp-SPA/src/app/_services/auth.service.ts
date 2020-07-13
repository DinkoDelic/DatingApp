import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  // url that we use in our login request
  baseUrl = environment.apiUrl + 'auth/';
  jwtHelper = new JwtHelperService();
  decodedToken: any;

  constructor(private http: HttpClient) {}

  login(model: any) {
    return this.http.post(this.baseUrl + 'login', model).pipe(
      map((response: any) => {
        const user = response;
        if (user) {
          localStorage.setItem('token', user.token);
        }
      })
    );
  }
 


  register(model: any) {
    return this.http.post(this.baseUrl + 'register', model);
  }
  // Checking if token is expired using JwtHelperService from auth0
  loggedIn() {
    const token = localStorage.getItem('token');
    this.decodedToken = this.jwtHelper.decodeToken(token);
    // returns true if token not expired/ false if it did
    return !this.jwtHelper.isTokenExpired(token);
  }
}
