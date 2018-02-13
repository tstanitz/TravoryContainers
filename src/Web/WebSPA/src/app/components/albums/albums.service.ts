import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Rx';
import { HttpClient } from '@angular/common/http';


import { Album } from '../album';
import { Photo } from '../photo';
import { UserdataService } from '../userdata.service';

@Injectable()
export class AlbumsService {

  constructor(private http: HttpClient, private userdataservice: UserdataService) { }

  getAlbums(): Observable<Album[]> {
    return this.http.post<Album[]>("http://localhost:5001/api/v1/flickr/albums", this.userdataservice.getData());
  }

  getPhotoIds(id: number): Observable<number> {
    return this.http.post<number>(`http://localhost:5001/api/v1/flickr/photoset/${id}/photos`, this.userdataservice.getData());
  }

  getPhoto(id: number): Observable<Photo> {
    return this.http.post<Photo>(`http://localhost:5001/api/v1/flickr/photo/${id}`, this.userdataservice.getData());
  }
}


