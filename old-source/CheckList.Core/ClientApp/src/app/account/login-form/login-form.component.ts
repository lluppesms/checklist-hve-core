import { Subscription } from 'rxjs';
import { Component, Inject, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { Credentials } from '../../shared/models/credentials.interface';
import { AuthService } from "../../shared/services/auth.service";

@Component({
  selector: 'app-login-form',
  templateUrl: './login-form.component.html',
  styleUrls: ['./login-form.component.css']
})

export class LoginFormComponent implements OnInit, OnDestroy {
  private subscription: Subscription;
  errors: string;
  isRequesting: boolean;
  submitted: boolean = false;
  loginUserName: string;
  credentials: Credentials = { email: '', password: '' };

  constructor(private auth: AuthService, private router: Router, private activatedRoute: ActivatedRoute, @Inject('BASE_URL') baseUrl: string) { }

  ngOnInit() {
    //// subscribe to router event
    //  (param: any) => {
    //    //this.brandNew = false; // param['brandNew'];
    //    this.credentials.email = param['email'];
    //  };
    this.loginUserName = localStorage.getItem('userName');
  }

  ngOnDestroy() {
    //this.subscription.unsubscribe();
  }

  login({ value, valid }: { value: Credentials, valid: boolean }) {
    this.submitted = true;
    this.isRequesting = true;
    this.errors = '';
    if (valid) {
      this.auth.login(value.email, value.password).then(result => {
        if (localStorage.getItem('auth_token')) {
          localStorage.setItem('userName', this.credentials.email);
          this.router.navigate(['collections']);
        }
      })
    }
  }
}
