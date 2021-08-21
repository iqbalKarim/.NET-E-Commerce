import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  baseUrl = 'https://localhost:5001/api/';
  obj: any;
  loggedIn: boolean = false;
  constructor(private http: HttpClient) {}

  login(model: any) {
    this.obj = this.http.post(this.baseUrl + 'account/login', model);
    if (!!this.obj) {
      this.loggedIn = true;
    }
    return this.obj;
  }

  register(model: any) {
    return this.http.post(this.baseUrl + 'account/register', model);
  }
}
