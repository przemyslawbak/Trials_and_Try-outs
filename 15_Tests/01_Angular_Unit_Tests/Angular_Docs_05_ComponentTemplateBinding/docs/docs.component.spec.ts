/*
As minimal as this is, you decide to add a test to confirm that component actually displays the right content where you think it should.
*/
import { TestBed, ComponentFixture, async } from '@angular/core/testing';
import { DocsComponent } from './docs.component';

let component: DocsComponent;
let fixture: ComponentFixture<DocsComponent>;
let h1: HTMLElement;

beforeEach(() => {
  TestBed.configureTestingModule({
    declarations: [DocsComponent],
  });
  fixture = TestBed.createComponent(DocsComponent);
  component = fixture.componentInstance; // DocsComponent test instance
  h1 = fixture.nativeElement.querySelector('h1');
});

it('should display original title', () => {
  expect(h1.textContent).toContain(component.title);
});
