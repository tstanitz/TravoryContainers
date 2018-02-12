import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AlbumsComponent} from './albums/albums.component';

const routes: Routes = [
  { path: '', redirectTo:'/albums', pathMatch: 'full'},
  { path: 'albums', component: AlbumsComponent }
];

@NgModule({
  exports: [RouterModule],
  imports: [RouterModule.forRoot(routes)]
})
export class AppRoutingModule { }
