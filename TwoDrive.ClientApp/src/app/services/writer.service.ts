import { Injectable } from '@angular/core';
import 'rxjs/add/operator/map';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Writer, Element } from 'src/app/components/interfaces/interfaces.model';

@Injectable({
  providedIn: 'root'
})
export class WriterService {

  public url: string = "http://localhost:3682";
  public LoggedInWriterId = +localStorage.getItem('writerId');
  private LoggedInWriter: Writer;
  private writers: Writer[];

  private readonly endpoint = this.url + '/api/Writer';

  constructor(private http: HttpClient) { }

  public CreateWriter(writer: Writer) {
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

  public UpdateWriter(writer: any) {
    var writerToken = localStorage.getItem('token');
    let headers = new HttpHeaders();
    headers = headers
    .set('Content-Type', 'application/json')
    .set('Authorization', writerToken);

    return this.http.put(this.endpoint + '/' + writer.id + '/', writer, {
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

  public GetLoggedInWriter(){
    return this.GetWriter(this.LoggedInWriterId);
  }

  public SetLoggedInWriter(writer: Writer){
    this.LoggedInWriter = writer;
  }

  public GetWriter(writerId: number){
    var writerToken = localStorage.getItem('token');
    let headers = new HttpHeaders();
    headers = headers
    .set('Content-Type', 'application/json')
    .set('Authorization', writerToken);

    return this.http.get(this.endpoint + '/' + writerId, {
      headers: headers
    });
  }

  public ParseGetAllWritersResponse(responseString : string){

    var responseParsed = JSON.parse(responseString);
    this.writers = responseParsed.filter(w => w.id !== this.LoggedInWriterId);
    this.writers.forEach(writer => {
      writer.role = this.GetWriterRole(writer);
      writer.isFriendsWithUserLoggedIn = this.GetIfWriterIsFriendsWithUserLoggedIn(writer);
    });
    return this.writers
  }

  private GetWriterRole(writer: any){
    if(writer.role == '0'){
      writer.role = "Writer"
    }
    else
    {
      writer.role = "Administrator"
    }
    return writer.role;
  }

  private GetIfWriterIsFriendsWithUserLoggedIn(writer: any){
    return !!this.LoggedInWriter.friends.find(f => +f.id == writer.id);
  }

  public AddFriend(id: number){
    var writerToken = localStorage.getItem('token');
    let headers = new HttpHeaders();
    headers = headers
    .set('Content-Type', 'application/json')
    .set('Authorization', writerToken);

    return this.http.put(this.endpoint + '/Friend/' + id, {}, {
      headers: headers,
      responseType: 'text'
    });
  }

  public RemoveFriend(id: number){
    var writerToken = localStorage.getItem('token');
    let headers = new HttpHeaders();
    headers = headers
    .set('Content-Type', 'application/json')
    .set('Authorization', writerToken);

    return this.http.delete(this.endpoint + '/Unfriend/' + id, {
      headers: headers,
      responseType: 'text'
    });
  }

  public DeleteWriter(id: number){
    var writerToken = localStorage.getItem('token');
    let headers = new HttpHeaders();
    headers = headers
    .set('Content-Type', 'application/json')
    .set('Authorization', writerToken);

    return this.http.delete(this.endpoint + '/' + id, {
      headers: headers,
      responseType: 'text'
    });
  }

  public GetElementsFromWriter(writer: Writer){
    var elements = writer.claims.map(a => a.element);
    elements = this.SetElementsPath(elements, writer.id);
    return elements;
  }

  private SetElementsPath(elements: Element[], id: number){
    var index = 0;
    var elementsToReturn = elements;
    elements.forEach(e =>{
      if(e.ownerId == id){
        elementsToReturn[index] = this.SetElementPath(e,'');
      }
      index++;
    })
    return elementsToReturn;
  }

  private SetElementPath(element: Element, path : string){
    element.path = path.concat('/' + element.name);
    if(element.isFolder){
      element.folderChildren.forEach(c => this.SetElementPath(c, element.path));
    }
    return element;
  }
}