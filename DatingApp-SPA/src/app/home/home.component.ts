import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { AlertModule } from 'ngx-bootstrap/alert';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit {
  registerMode = false;
  infoMessage = false;

  constructor(private http: HttpClient) {}

  ngOnInit() {}
  // method to display register mode
  registerToggle() {
    this.registerMode = true;
  }

  cancelRegisterMode(registerMode: boolean) {
    this.registerMode = registerMode;
  }
  
  learnMore() {
    if (this.infoMessage)
    {
      this.infoMessage = false;
    }

    else
    {
      this.infoMessage = true;
    }
      
  }
}
