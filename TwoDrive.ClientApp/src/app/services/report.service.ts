import { Injectable } from '@angular/core';
import 'rxjs/add/operator/map';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Writer } from 'src/app/components/interfaces/interfaces.model';

@Injectable({
  providedIn: 'root'
})
export class ReportService {

  public url: string = "http://localhost:3682";

  private readonly endpoint = this.url + '/api/Report';

  constructor(private http: HttpClient) { }

  private CreateWriter(writer: Writer) {
    var writerToken = localStorage.getItem('token');
    let headers = new HttpHeaders();
    headers = headers
    .set('Content-Type', 'application/json')
    .set('Authorization', writerToken);

    return this.http.post(this.endpoint, writer, {
      headers: headers,
      responseType: 'text'
    });
  }

  public GetTopWriters(){
    var writerToken = localStorage.getItem('token');
    let headers = new HttpHeaders();
    headers = headers
    .set('Content-Type', 'application/json')
    .set('Authorization', writerToken);

    return this.http.get(this.endpoint, {
        headers: headers
    });
  }
}