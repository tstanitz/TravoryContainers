import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Rx';
import { of } from 'rxjs/observable/of';

import {Album} from './album';

@Injectable()
export class AlbumsService {

  albums: Album[] = [
    { id: 1 },
    { id: 2 }
  ];
  constructor() { }

  getAlbums(): Observable<Album[]> {   
    return of(this.albums);
  }
}


