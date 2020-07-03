import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { error } from '@angular/compiler/src/util';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  // passing value from parent component, name has to match
  @Input() valuesFromHome: any;
  @Output() cancelRegister = new EventEmitter();
  // empty object
  model: any = {};

  constructor(private authService: AuthService) { }

  ngOnInit() {
  }

  register() {
    this.authService.register(this.model).subscribe(() => {
      console.log('registration successful');
    }, error => {console.log(error);
    });
  }

  cancel() {
    // Emitting an output value (it can be anything type)
    this.cancelRegister.emit(false);
  }

}
