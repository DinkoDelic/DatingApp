import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  // url that we use in our login request
  baseUrl = environment.apiUrl + 'auth/';
  jwtHelper = new JwtHelperService();
  currentUser: User;
  decodedToken: any;
  // Behaviour subject needs to have initial value
  photoUrl = new BehaviorSubject<string>('../../assets/user.png');
  currentPhotoUrl = this.photoUrl.asObservable();

  constructor(private http: HttpClient) {}

  changeMainPhoto(photoUrl: string){
    this.photoUrl.next(photoUrl);
  }

  login(model: any) {
    return this.http.post(this.baseUrl + 'login', model).pipe(
      map((response: any) => {
        const user = response;
        if (user) {
          localStorage.setItem('token', user.token);
          localStorage.setItem('user', JSON.stringify(user.user));
          this.currentUser = user.user;
          this.changeMainPhoto(this.currentUser.photoUrl);
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
