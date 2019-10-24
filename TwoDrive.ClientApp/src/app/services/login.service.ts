import { Injectable } from '@angular/core';
import 'rxjs/add/operator/map';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  private loggedIn = new BehaviorSubject<boolean>(false);
  public url: string = "http://localhost:3682";
  private readonly loginEndpoint = this.url +'/api/Token';

  constructor(private http: HttpClient) { }

  public Login(credentials : any)
  {
    this.loggedIn.next(true);
    return this.http.post(this.loginEndpoint, credentials);
  }

  get isAuthenticated(){
    const token = localStorage.getItem('token');
    return this.loggedIn.asObservable();
  }

}