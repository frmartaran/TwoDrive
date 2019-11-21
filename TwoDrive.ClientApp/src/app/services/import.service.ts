import { Injectable } from '@angular/core';
import 'rxjs/add/operator/map';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
    providedIn: 'root'
})

export class ImportService {

    public url: string = "http://localhost:3682";

    private readonly endpoint = this.url + '/api/Import';

    constructor(private http: HttpClient) { }

    public getImporters() {
        var writerToken = localStorage.getItem('token');
        let headers = new HttpHeaders();
        headers = headers
            .set('Content-Type', 'application/json')
            .set('Authorization', writerToken);

        return this.http.get(this.endpoint, {
            headers: headers,
        })
    }

    public import(id: number, importer: string, path: string) {
        var writerToken = localStorage.getItem('token');
        let headers = new HttpHeaders();
        headers = headers
            .set('Content-Type', 'application/json')
            .set('Authorization', writerToken);

        var endpoint = this.endpoint + "/" + importer + "/" + id;
        return this.http.post(endpoint, { path }, {
            headers: headers,
            responseType: 'text'
        });
    }
}