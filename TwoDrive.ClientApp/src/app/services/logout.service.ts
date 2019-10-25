import { Injectable } from '@angular/core';
import 'rxjs/add/operator/map';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class LogoutService {

  public url: string = "http://localhost:3682";
  private readonly logoutEndpoint = this.url +'/api/Token';

  constructor(private http: HttpClient) { }

  public Logout()
  {
    return this.http.delete(this.logoutEndpoint);
  }
}