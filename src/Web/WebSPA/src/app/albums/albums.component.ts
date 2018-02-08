import { Component, OnInit } from '@angular/core';
import { AlbumsService } from './albums.service';
import {Album} from './album';

@Component({
  selector: 'app-albums',
  templateUrl: './albums.component.html',
  styleUrls: ['./albums.component.css']
})
export class AlbumsComponent implements OnInit {

  public albums: Album[];
  constructor(private albumsService: AlbumsService) { }

  ngOnInit() {
    this.albumsService.getAlbums().subscribe(a => this.albums = a);
  }

}
