import { Injectable } from '@angular/core';

@Injectable()
export class DocsService {
  isLoggedIn: any;
  user: any;
  constructor() {
    this.isLoggedIn = true;
    this.user.name = 'Miron';
  }
}
