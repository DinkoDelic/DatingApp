import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';


@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  // object to store username and pass
  model: any = {};

  constructor(private authService: AuthService) { }

  ngOnInit() { }

  login() {
    this.authService.login(this.model).subscribe(next => {
      console.log('Logged in successfully');
    }, error => {
      console.log(error);
    });
  }
  loggedIn() {
    const token = localStorage.getItem('token');
    // !! used to return a boolean value
    return !!token;
  }

  logOut() {
    localStorage.removeItem('token');
    console.log('Logged out');
  }

}
