import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { LoginService } from 'src/app/services/login.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  title = 'app';
  isAuthenticated$: Observable<boolean>; 

  constructor(private loginService: LoginService) { }

  ngOnInit() {
    this.isAuthenticated$ = this.loginService.getIsAuthenticated;
  }
  
}