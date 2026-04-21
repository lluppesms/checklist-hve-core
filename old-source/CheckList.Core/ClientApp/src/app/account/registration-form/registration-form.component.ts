import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from "../../shared/services/auth.service";
import { UserRegistration } from '../../shared/models/user.registration.interface';

@Component({
  selector: 'app-registration-form',
  templateUrl: './registration-form.component.html',
  styleUrls: ['./registration-form.component.css']
})

export class RegistrationFormComponent implements OnInit {

  errors: string;
  isRequesting: boolean;
  submitted: boolean = false;

  constructor(private auth: AuthService, private router: Router) { }

  ngOnInit() {
  }
  registerUser({ value, valid }: { value: UserRegistration, valid: boolean }) {
    this.submitted = true;
    this.isRequesting = true;
    this.errors = '';
    if (valid) {
      this.auth.register(value.email, value.password, value.firstName, value.lastName, value.location)
        .then(result => {
          this.router.navigate(['/login'], { queryParams: { email: value.email } });
        });
    }
  }
}
