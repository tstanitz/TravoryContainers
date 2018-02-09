import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Rx';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../environments/environment';

import { Album } from './album';
import { Photo } from './photo';

@Injectable()
export class AlbumsService {

  constructor(private http: HttpClient) { }

  getAlbums(): Observable<Album[]> {
    return this.http.post<Album[]>("http://localhost:5001/api/v1/flickr/albums", {
      "consumerKey": environment.consumerKey,
      "consumerSecret": environment.consumerSecret,
      "token": environment.token,
      "tokenSecret": environment.tokenSecret
    });
  }

  getPhotoIds(id: number): Observable<number> {
    return this.http.post<number>(`http://localhost:5000/api/v1/flickr/photoset/${id}/photos`,
      {
        "consumerKey": environment.consumerKey,
        "consumerSecret": environment.consumerSecret,
        "token": environment.token,
        "tokenSecret": environment.tokenSecret
      });
  }

  getPhoto(id: number): Observable<Photo> {
    return this.http.post<Photo>(`http://localhost:5000/api/v1/flickr/photo/${id}`,
      {
        "consumerKey": environment.consumerKey,
        "consumerSecret": environment.consumerSecret,
        "token": environment.token,
        "tokenSecret": environment.tokenSecret
      });
  }
}


