import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';

import { AlbumsService } from '../albums.service';
import {Photo} from "../photo";

@Component({
  selector: 'app-album-detail',
  templateUrl: './album-detail.component.html',
  styleUrls: ['./album-detail.component.css']
})
export class AlbumDetailComponent implements OnInit {
  photos: Photo[] = [];
  constructor(
    private route: ActivatedRoute,
    private albumService: AlbumsService,
    private location: Location
  ) { }

  ngOnInit() {
    let photoSetId = this.route.snapshot.paramMap.get('id');
    this.albumService.getPhotoIds(photoSetId).subscribe(ids => {
      for (let i of ids) {
        this.albumService.getPhoto(i.id).subscribe(p => this.photos.push(p));      
      }
    });
  }

}
