import { Injectable } from '@angular/core';
import 'rxjs/add/operator/map';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  private isAuthenticated = new BehaviorSubject<boolean>(false);
  public url: string = "http://localhost:3682";
  private readonly loginEndpoint = this.url +'/api/Token';
  private session: any = {
    userId: '',
    token: '',
    isAdmin: false
  };

  constructor(private http: HttpClient) { }

  public Login(credentials : any)
  {
    return this.http.post(this.loginEndpoint, credentials);
  }

  authenticateUser(response : Object){
    var responseInJson = JSON.stringify(response);
    this.session = JSON.parse(responseInJson);
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