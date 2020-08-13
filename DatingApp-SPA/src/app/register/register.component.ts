import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import {
  FormGroup,
  FormControl,
  Validators,
  RequiredValidator,
  FormBuilder,
} from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { User } from '../_models/user';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent implements OnInit {
  // passing value from parent component, name has to match
  @Input() valuesFromHome: any;
  // returning value to parent component
  @Output() cancelRegister = new EventEmitter();
  // empty object
  user: User;
  registerForm: FormGroup;
  bsConfig: Partial<BsDatepickerConfig>;
  minDate: Date;
  maxDate: Date;

  constructor(
    private authService: AuthService,
    private alertify: AlertifyService,
    private fb: FormBuilder,
    private router: Router
  ) 
  {
    // Sets minimun date to 118 years ago an earliest date to 18 years ago
    this.minDate = new Date();
    this.maxDate = new Date();
    this.minDate.setFullYear(this.minDate.getFullYear() - 118);
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 18 );
  }

  ngOnInit() {
    (this.bsConfig = {
      isAnimated: true,
      containerClass: 'theme-blue',
    }),
      this.createRegisterForm();
  }

  createRegisterForm() {
    this.registerForm = this.fb.group(
      {
        // radio button
        gender: ['male'],
        username: ['', Validators.required],
        knownAs: ['', Validators.required],
        dateOfBirth: [null, Validators.required],
        city: ['', Validators.required],
        country: ['', Validators.required],
        password: [
          '',
          [
            Validators.required,
            Validators.minLength(4),
            Validators.maxLength(8),
          ],
        ],
        confirmPassword: ['', Validators.required],
      },
      { validators: this.passwordMatchValidator }
    );
  }

  passwordMatchValidator(g: FormGroup) {
    return g.get('password').value === g.get('confirmPassword').value
      ? null
      : { mismatch: true };
  }

  register() {
    if (this.registerForm.valid)
    {
      // Assigning form group values as an object to user
      this.user = Object.assign({}, this.registerForm.value);
     

      this.authService.register(this.user).subscribe(
        () => {
          this.alertify.success('Registration successful');
        },
        (error) => {
          this.alertify.error(error);
        },
        () =>
          this.authService.login(this.user).subscribe(() => {
            this.router.navigate(['/members']);
          })
      );
    }
  }

  cancel() {
    // Emitting an output value (it can be anything type), takes us back from registry menu
    this.cancelRegister.emit(false);
  }
}
