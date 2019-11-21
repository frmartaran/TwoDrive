import { Injectable } from '@angular/core';
import 'rxjs/add/operator/map';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Writer } from 'src/app/components/interfaces/interfaces.model';
import { environment } from './../../environments/environment.prod';

@Injectable({
  providedIn: 'root'
})
export class ReportService {

  public url: string = environment.apiUrl;

  private readonly endpoint = this.url + 'api/Report';

  constructor(private http: HttpClient) { }

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

  public GetModificationsReport(ReportType: string, StartDate: Date, EndDate: Date) {
    var writerToken = localStorage.getItem('token');
    let headers = new HttpHeaders();
    headers = headers
      .set('Content-Type', 'application/json')
      .set('Authorization', writerToken);

    var endpoint = this.endpoint + "/" + ReportType + "/Modifications";
    return this.http.post(endpoint, {
      StartDate,
      EndDate
    },
      {
        headers: headers,
        responseType: 'text'
      });
  }
}