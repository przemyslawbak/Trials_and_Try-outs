import {
  TestBed,
  ComponentFixture,
  tick,
  fakeAsync,
} from '@angular/core/testing';
import { interval, of, throwError } from 'rxjs';
import { delay, take } from 'rxjs/operators';
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
/*The following test confirms the expected behavior when the service returns an ErrorObservable.*/

it('should display error when TwainService fails', fakeAsync(() => {
  // tell spy to return an error observable
  getQuoteSpy.and.returnValue(throwError('TwainService test failure'));

  fixture.detectChanges(); // onInit()
  // sync spy errors immediately after init

  tick(); // flush the component's setTimeout()

  fixture.detectChanges(); // update errorMessage within setTimeout()
  expect(quoteEl.textContent).toBe('...', 'should show placeholder');
}));

//tick
it('should get Date diff correctly in fakeAsync', fakeAsync(() => {
  const start = Date.now();
  tick(100);
  const end = Date.now();
  expect(end - start).toBe(100);
}));
