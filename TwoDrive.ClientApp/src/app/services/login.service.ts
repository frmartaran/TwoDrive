import { Injectable } from '@angular/core';
import 'rxjs/add/operator/map';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import { environment } from './../../environments/environment.prod';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  private isAuthenticated = new BehaviorSubject<boolean>(false);
  public url: string = environment.apiUrl;
  private readonly loginEndpoint = this.url +'api/Token';
  private session: any = {
    userId: '',
    token: '',
    isAdmin: false
  };

  constructor(private http: HttpClient) { }

  public Login(credentials : any)
  {
    return this.http.post(this.loginEndpoint, credentials, {
      responseType: 'text'
    });
  }

  authenticateUser(response : string){
    this.session = JSON.parse(response);
    localStorage.setItem("token", this.session.token);
    localStorage.setItem("writerId", this.session.userId);
    localStorage.setItem("isAdmin", this.session.isAdmin);
    localStorage.setItem("username", this.session.username);
    this.isAuthenticated.next(true);
  }

  get getIsAuthenticated(){
    return this.isAuthenticated; 
  }

}