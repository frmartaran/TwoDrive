import { Injectable } from '@angular/core';
import 'rxjs/add/operator/map';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ElementService {

  public url: string = "http://localhost:3682";

  private readonly endpoint = this.url + '/api/Folder';

  constructor(private http: HttpClient) { }

  public GetFolder(id: number){
    var writerToken = localStorage.getItem('token');
    let headers = new HttpHeaders();
    headers = headers
    .set('Content-Type', 'application/json')
    .set('Authorization', writerToken);

    return this.http.get(this.endpoint + '/' + id, {
        headers: headers,
      });
  }
}