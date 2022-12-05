import { TestBed, ComponentFixture } from '@angular/core/testing';
import { of } from 'rxjs';
import { DocsComponent } from './docs.component';
import { DocsService } from './docs.service';
import { TwainService } from './twain.service';

let quoteEl: HTMLElement;
let fixture: ComponentFixture<DocsComponent>;
let component: DocsComponent;
let testQuote: string = 'dupa';
let getQuoteSpy: any;

beforeEach(() => {
  testQuote = 'Test Quote';

  // Create a fake TwainService object with a `getQuote()` spy
  const twainService = jasmine.createSpyObj('TwainService', ['getQuote']);
  // Make the spy return a synchronous Observable with the test data
  getQuoteSpy = twainService.getQuote.and.returnValue(of(testQuote));

  TestBed.configureTestingModule({
    declarations: [DocsComponent],
    providers: [{ provide: TwainService, useValue: twainService }],
  });

  fixture = TestBed.createComponent(DocsComponent);
  component = fixture.componentInstance;
  quoteEl = fixture.nativeElement.querySelector('.twain');
});
/*The spy is designed such that any call to getQuote receives an observable with a test quote. 
Unlike the real getQuote() method, this spy bypasses the server and returns a synchronous 
observable whose value is available immediately.

A key advantage of a synchronous Observable is that you can often turn asynchronous 
processes into synchronous tests.*/

it('should show quote after component initialized', () => {
  fixture.detectChanges(); // onInit()

  // sync spy result shows testQuote immediately after init
  expect(quoteEl.textContent).toBe(testQuote);
  expect(getQuoteSpy.calls.any()).toBe(true, 'getQuote called');
});
