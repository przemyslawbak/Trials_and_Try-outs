import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable()
export class TwainService {
  getQuote(): Observable<string> {
    throw new Error('Method not implemented.');
  }
  constructor() {}
}
