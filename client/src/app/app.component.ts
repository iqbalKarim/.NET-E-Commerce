import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { User } from './_model/user';
import { AccountService } from './_service/account.service';
import { ProductService } from './_service/product.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  title = 'E-Commerce';
  users: any;
  products: any;
  model: any = {};
  register: any = {};
  loggedIn: boolean = false;
  registerError: number = 0;
  loginError: boolean = false;

  constructor(
    private http: HttpClient,
    private productServices: ProductService,
    private accountServices: AccountService
  ) {}

  ngOnInit(): void {
    this.getAllProducts();
  }

  login() {
    console.log('login');
    this.accountServices.login(this.model).subscribe(
      (response: User) => {
        console.log(response);
        this.loggedIn = true;
        this.loginError = false;
      },
      (error: any) => {
        console.log(error);
        this.loginError = true;
      }
    );
  }

  logout() {
    this.loggedIn = false;
  }

  getAllProducts() {
    this.productServices.getAllProducts().subscribe(
      (responce) => {
        console.log(responce);
        this.products = responce;
      },
      (error) => {
        console.log(error);
      }
    );
  }

  getProducts(category: string) {
    this.productServices.getProducts(category).subscribe(
      (response) => {
        this.products = response;
      },
      (error) => {
        console.log(error);
      }
    );
  }

  registerUser() {
    console.log('register');
    this.accountServices.register(this.register).subscribe(
      (response) => {
        console.log(response);
        this.registerError = 2;
      },
      (error) => {
        console.log(error);
        this.registerError = 1;
      }
    );
  }
}
