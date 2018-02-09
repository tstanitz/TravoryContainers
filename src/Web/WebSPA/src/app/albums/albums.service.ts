import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Rx';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import {Album} from './album';

@Injectable()
export class AlbumsService {

  constructor(private http: HttpClient) { }

  getAlbums(): Observable<Album[]> {
    return this.http.post<Album[]>("http://localhost:5000/api/v1/flickr/albums", {});
  }
}


