import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from "../_services/alertify.service";


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  // passing value from parent component, name has to match
  @Input() valuesFromHome: any;
  // returning value to parent component
  @Output() cancelRegister = new EventEmitter();
  // empty object
  model: any = {};

  constructor(public authService: AuthService, private alertify: AlertifyService) { }

  ngOnInit() {
  }

  register() {
    this.authService.register(this.model).subscribe(() => {
      this.alertify.success('Registration successful');
    }, error => {this.alertify.error(error);
    });
  }

  cancel() {
    // Emitting an output value (it can be anything type), takes us back from registry menu
    this.cancelRegister.emit(false);
  }

}
