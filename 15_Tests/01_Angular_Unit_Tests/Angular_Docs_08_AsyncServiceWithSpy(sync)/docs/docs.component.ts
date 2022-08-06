import { Component, OnInit } from '@angular/core';
import { of } from 'rxjs';
import { startWith, catchError } from 'rxjs/operators';
import { TwainService } from './twain.service';
//https://angular.io/guide/testing-components-scenarios#testing-with-a-spy
@Component({
  selector: 'docs',
  template: ` <p class="twain">
      <i>{{ quote | async }}</i>
    </p>
    <button (click)="getQuote()">Next quote</button>
    <p class="error" *ngIf="errorMessage">{{ errorMessage }}</p>`,
  styleUrls: ['./docs.component.css'],
})
export class DocsComponent implements OnInit {
  public errorMessage: string = '';
  public quote: any = 'dupa';

  constructor(private twainService: TwainService) {}

  ngOnInit(): void {
    this.getQuote();
  }

  getQuote() {
    this.errorMessage = '';
    this.quote = this.twainService.getQuote().pipe(
      startWith('...'),
      catchError((err: any) => {
        // Wait a turn because errorMessage already set once this turn
        setTimeout(() => (this.errorMessage = err.message || err.toString()));
        return of('...'); // reset message to placeholder
      })
    );
  }
}
