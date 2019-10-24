import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { LoginService } from './services/login.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  title = 'app';
  isLoggedIn: Observable<boolean>; 

  constructor(private loginService: LoginService) { }

  ngOnInit() {
    this.isLoggedIn = this.loginService.isAuthenticated;
  }
  
}
