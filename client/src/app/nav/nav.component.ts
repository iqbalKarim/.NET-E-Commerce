import { Component, OnInit } from '@angular/core';
import { User } from '../_model/user';
import { AccountService } from '../_service/account.service';
import { ProductService } from '../_service/product.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css'],
})
export class NavComponent implements OnInit {
  model: any = {};
  loggedIn: boolean = this.accountServices.loggedIn;

  constructor(private accountServices: AccountService) {}

  login() {
    this.accountServices.login(this.model).subscribe(
      (response: User) => {
        console.log(response);
        this.loggedIn = true;
      },
      (error: any) => {
        console.log(error);
      }
    );
  }

  logout() {
    this.loggedIn = false;
  }

  ngOnInit(): void {}
}
