import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { AlbumsComponent } from './albums/albums.component';
import { AlbumsService } from './albums/albums.service';


@NgModule({
  declarations: [
    AppComponent,
    AlbumsComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule
  ],
  providers: [AlbumsService],
  bootstrap: [AppComponent]
})
export class AppModule { }
