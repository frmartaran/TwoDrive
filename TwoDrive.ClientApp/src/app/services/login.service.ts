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

  constructor(private http: HttpClient) { }

  public Login(credentials : any)
  {
    return this.http.post(this.loginEndpoint, credentials);
  }

  authenticateUser(response : Object){
    localStorage.setItem("token", JSON.stringify(response));
    this.isAuthenticated.next(true);
  }

  get getIsAuthenticated(){
    return this.isAuthenticated; 
  }

}