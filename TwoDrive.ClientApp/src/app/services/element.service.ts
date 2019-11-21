import { Injectable } from '@angular/core';
import 'rxjs/add/operator/map';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Writer, Element } from 'src/app/components/interfaces/interfaces.model';

@Injectable({
  providedIn: 'root'
})
export class ElementService {

  public url: string = "http://localhost:3682";

  private readonly folderEndpoint = this.url + '/api/Folder';
  private readonly fileEndpoint = this.url + '/api/File';
  private elementFound: Element;

  constructor(private http: HttpClient) { }

  public GetFolder(id: number) {
    var writerToken = localStorage.getItem('token');
    let headers = new HttpHeaders();
    headers = headers
      .set('Content-Type', 'application/json')
      .set('Authorization', writerToken);

    return this.http.get(this.folderEndpoint + '/' + id, {
      headers: headers,
      responseType: 'text'
    });
  }

  public GetAllFiles() {
    var writerToken = localStorage.getItem('token');
    let headers = new HttpHeaders();
    headers = headers
      .set('Content-Type', 'application/json')
      .set('Authorization', writerToken);

    return this.http.get(this.fileEndpoint, {
      headers: headers,
    });
  }

  public MoveFolder(folderToMoveId: number, folderDestinationId: number) {
    var writerToken = localStorage.getItem('token');
    let headers = new HttpHeaders();
    headers = headers
      .set('Content-Type', 'application/json')
      .set('Authorization', writerToken);

    return this.http.post(this.folderEndpoint + '/' + folderToMoveId + '/' + folderDestinationId, {} , {
        headers: headers,
        responseType: 'text'
      });
  }

  public MoveFile(fileToMove: number, folderDestinationId: number){
    var writerToken = localStorage.getItem('token');
    let headers = new HttpHeaders();
    headers = headers
    .set('Content-Type', 'application/json')
    .set('Authorization', writerToken);

    return this.http.post(this.fileEndpoint + '/' + fileToMove + '/' + folderDestinationId, {} , {
        headers: headers,
        responseType: 'text'
      });
  }

  public GetElementFromPath(path: string, element: Element){
    this.elementFound = null;
    this.GetElementFromPathRecursive(path, element);
    if(this.elementFound != null && this.elementFound.isFolder){
      return this.elementFound;
    }else{
      return null
    }    
  }

  private GetElementFromPathRecursive(path: string, element: Element){
    if(element == null){
      return null;
    }
    if(element.path === path) {
      this.elementFound = element;
      return element;
    }
    if(element.isFolder){
      element.folderChildren.forEach(c => {
        return this.GetElementFromPathRecursive(path, c)  
      });
    }else{
      return element
    }
  }

  public AddFolder(ownerId: number, parentFolderId: number, name: string){
    var writerToken = localStorage.getItem('token');
    let headers = new HttpHeaders();
    headers = headers
    .set('Content-Type', 'application/json')
    .set('Authorization', writerToken);
    var folderToAdd: any = {
      name: name,
      ownerId: ownerId
    }

    return this.http.post(this.folderEndpoint + '/' + parentFolderId, folderToAdd , {
        headers: headers,
        responseType: 'text'
      });
  }

  public ShareElement(elementId: number, friendId: number, isFolder: boolean){
    var writerToken = localStorage.getItem('token');
    let headers = new HttpHeaders();
    headers = headers
    .set('Content-Type', 'application/json')
    .set('Authorization', writerToken);

    var endpointToSend = isFolder
      ? this.folderEndpoint
      : this.fileEndpoint;

    return this.http.put(endpointToSend + '/' + elementId + '/' + friendId, {} , {
        headers: headers,
        responseType: 'text'
      });
  }

  public StopShareElement(elementId: number, friendId: number, isFolder: boolean){
    var writerToken = localStorage.getItem('token');
    let headers = new HttpHeaders();
    headers = headers
    .set('Content-Type', 'application/json')
    .set('Authorization', writerToken);

    var endpointToSend = isFolder
      ? this.folderEndpoint
      : this.fileEndpoint;

    return this.http.delete(endpointToSend + '/' + elementId + '/' + friendId, {
        headers: headers,
        responseType: 'text'
      });
  }

  public Delete(id: number, isFolder: boolean) {
    var writerToken = localStorage.getItem('token');
    let headers = new HttpHeaders();
    headers = headers
      .set('Content-Type', 'application/json')
      .set('Authorization', writerToken);

    var endpoint = this.fileEndpoint + "/" + id;
    if (isFolder)
      endpoint = this.folderEndpoint + "/" + id;
    return this.http.delete(endpoint, {
      headers: headers,
      responseType: 'text'
    });
  }
}