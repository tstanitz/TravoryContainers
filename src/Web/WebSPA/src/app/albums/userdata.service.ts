import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';

import {UserData} from './userdata'

@Injectable()
export class UserdataService {
  userData: UserData = {
    consumerKey: environment.consumerKey,
    consumerSecret: environment.consumerSecret,
    token: environment.token,
    tokenSecret: environment.tokenSecret
  };

  constructor() { }

  getData(): UserData {   
    return this.userData;
  }

}
