import { Injectable } from '@angular/core';
import 'rxjs/add/operator/map';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class WriterService {

  public url: string = "http://localhost:3682";
  public writerLoggedInId = +localStorage.getItem('writerId');

  private readonly endpoint = this.url + '/api/Writer';

  constructor(private http: HttpClient) { }

  public CreateWriter(writer: any) {
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

  public GetAllWriters(){
    var writerToken = localStorage.getItem('token');
    let headers = new HttpHeaders();
    headers = headers
    .set('Content-Type', 'application/json')
    .set('Authorization', writerToken);

    return this.http.get(this.endpoint, {
      headers: headers
    });
  }

  public ParseGetAllWritersResponse(responseString : string){

    var responseParsed = JSON.parse(responseString);
    responseParsed = responseParsed.filter(w => w.id !== this.writerLoggedInId);
    responseParsed.forEach(writer => {
      if(writer.role == '0'){
        writer.role = "Writer"
      }
      else
      {
        writer.role = "Administrator"
      }
    });
    return responseParsed
  }
}