import { LoginService } from 'src/app/services/login.service';
import { Component, OnInit,} from '@angular/core';
import { Router } from '@angular/router';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})

export class LoginComponent implements OnInit {

  constructor(private router: Router,
    private loginService: LoginService) { }

  public credentials: any = {
    username: "",
    password: "",
  };

  public authenticationError: string = "";

  public user: any = {};

  ngOnInit() {
  }

  public Login(loginForm: NgForm) {
    this.credentials.username = loginForm.value.username;
    this.credentials.password = loginForm.value.password;
    this.loginService.Login(this.credentials)
    .subscribe(
      response => {
        localStorage.setItem("token", JSON.stringify(response));
      },
      (error) => {
        this.authenticationError = error;
      },
      () => {
        this.router.navigate(['/home-page']);
      }
    )
  }
}
