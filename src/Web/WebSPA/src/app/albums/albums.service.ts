import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Rx';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../environments/environment';

import { Album } from './album';

@Injectable()
export class AlbumsService {

  constructor(private http: HttpClient) { }

  getAlbums(): Observable<Album[]> {
    return this.http.post<Album[]>("http://localhost:5001/api/v1/flickr/albums", {
      "consumerKey": environment.consumerKey,
      "consumerSecret": environment.consumerSecret,
      "token": environment.token,
      "tokenSecret": environment.tokenSecret});
  }
}


