import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule} from './app-routing.module'

import { AppComponent } from './app.component';
import { AlbumsComponent } from './components/albums.component';
import { AlbumsService } from './components/albums.service';
import { UserdataService } from './components/userdata.service';


@NgModule({
  declarations: [
    AppComponent,
    AlbumsComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule
  ],
  providers: [AlbumsService, UserdataService],
  bootstrap: [AppComponent]
})
export class AppModule { }
