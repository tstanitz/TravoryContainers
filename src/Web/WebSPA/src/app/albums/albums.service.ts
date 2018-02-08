import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Rx';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import {Album} from './album';

@Injectable()
export class AlbumsService {

  albums: Album[] = [
    { id: 1 },
    { id: 2 }
  ];
  constructor(private http: HttpClient) { }

  getAlbums(): Observable<Album[]> {
    return this.http.get<Album[]>("http://localhost:5001/api/v1/flickr/albums");
  }
}


