import { Injectable } from '@angular/core';
import 'rxjs/add/operator/map';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ElementService {

  public url: string = "http://localhost:3682";

  private readonly folderEndpoint = this.url + '/api/Folder';
  private readonly fileEndpoint = this.url + '/api/File';

  constructor(private http: HttpClient) { }

  public GetFolder(id: number){
    var writerToken = localStorage.getItem('token');
    let headers = new HttpHeaders();
    headers = headers
    .set('Content-Type', 'application/json')
    .set('Authorization', writerToken);

    return this.http.get(this.folderEndpoint + '/' + id, {
        headers: headers,
      });
  }

  public GetAllFiles(){
    var writerToken = localStorage.getItem('token');
    let headers = new HttpHeaders();
    headers = headers
    .set('Content-Type', 'application/json')
    .set('Authorization', writerToken);

    return this.http.get(this.fileEndpoint, {
        headers: headers,
      });
  }

  public MoveFolder(folderToMoveId: number, folderDestinationId: number){
    var writerToken = localStorage.getItem('token');
    let headers = new HttpHeaders();
    headers = headers
    .set('Content-Type', 'application/json')
    .set('Authorization', writerToken);

    var moveFolderBody : any = {
      folderToMoveId: folderToMoveId,
      folderDestinationId: folderDestinationId
    }

    return this.http.post(this.fileEndpoint, moveFolderBody, {
        headers: headers,
        responseType: 'text'
      });
  }
}