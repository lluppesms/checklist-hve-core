import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { LoginResult } from "../models/LoginResult.model";
import { Router } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';
import { UserRegistration } from '../../shared/models/user.registration.interface';
import { map } from "rxjs/operators";

const loginUrl = "api/auth/login";
const registerUrl = "api/auth/register";

@Injectable()
export class AuthService {
  private baseUrl: string;
  public isLoggedIn: boolean = false;
  public loggedInUserName: string = "";
  public token: string;
  //// private _authNavStatusSource.next(true);
  // Observable navItem source
  private _authNavStatusSource = new BehaviorSubject<boolean>(false);
  // Observable navItem stream
  authNavStatus$ = this._authNavStatusSource.asObservable();

  constructor(private http: HttpClient, private router: Router, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }
  public getBearerToken() {
    let token = localStorage.getItem('auth_token');
    if (!token) {
      this._authNavStatusSource.next(false);
      this.router.navigate(['login']);
      return "";
    }
    let bearerToken = "Bearer " + token
    return bearerToken;
  }

  public login(userName, password) {
    return this.http
      .post<LoginResult>(this.baseUrl + loginUrl, JSON.stringify({ userName, password }), { headers: { 'Content-Type': 'application/json' } })
      .pipe(map(result => {
        this.isLoggedIn = true;
        this.loggedInUserName = userName;
        this.token = result.auth_token;
        localStorage.setItem('auth_token', result.auth_token);
        localStorage.setItem('userName', userName);
        this._authNavStatusSource.next(true);
        return true;
      })).toPromise();
  }

  public logout() {
    this.isLoggedIn = true;
    this.loggedInUserName = "";
    this.token = "";
    localStorage.removeItem('auth_token');
    localStorage.removeItem('userName');
    this.router.navigate(['login']);
    this._authNavStatusSource.next(false);
  }

  public register(email: string, password: string, firstName: string, lastName: string, location: string) {
    let body = JSON.stringify({ email, password, firstName, lastName, location });
    return this.http.post<UserRegistration>(this.baseUrl + registerUrl, body, { headers: { 'Content-Type': 'application/json' } })
      .pipe(map(result => { return true; })).toPromise();
  }
}

