import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Rx';
import { HttpClient } from '@angular/common/http';


import { Album } from './album';
import { Photo } from './photo';
import { UserdataService } from './userdata.service';

@Injectable()
export class AlbumsService {
  apiUrl = "http://localhost:30303";
  constructor(private http: HttpClient, private userdataservice: UserdataService) { }

  getAlbums(): Observable<Album[]> {
    return this.http.post<Album[]>(`${this.apiUrl}/api/v1/flickr/albums`, this.userdataservice.getData());
  }

  getPhotoIds(id: string): Observable<number[]> {
    return this.http.post<number[]>(`${this.apiUrl}/api/v1/flickr/photoset/${id}/photos`, this.userdataservice.getData());
  }

  getPhoto(id: number): Observable<Photo> {
    return this.http.post<Photo>(`${this.apiUrl}/api/v1/flickr/photo/${id}`, this.userdataservice.getData());
  }
}


