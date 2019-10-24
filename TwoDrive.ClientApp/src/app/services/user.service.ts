import { Injectable } from '@angular/core';
import { HttpHeaders } from '@angular/common/http';
import 'rxjs/add/operator/map';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class UserService {

  public url: string = "http://localhost:3682";

  private readonly endpoint = this.url + '/api/users';

  constructor(private http: HttpClient) { }

}