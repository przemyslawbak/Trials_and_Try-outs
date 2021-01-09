import {
  TestBed,
  ComponentFixture,
  ComponentFixtureAutoDetect,
} from '@angular/core/testing';
import { DocsComponent } from './docs.component';

let component: DocsComponent;
let fixture: ComponentFixture<DocsComponent>;
let h1: HTMLElement;

//https://angular.io/guide/testing-components-scenarios#automatic-change-detection
beforeEach(() => {
  TestBed.configureTestingModule({
    declarations: [DocsComponent],
    providers: [{ provide: ComponentFixtureAutoDetect, useValue: true }],
  });
  fixture = TestBed.createComponent(DocsComponent);
  component = fixture.componentInstance; // DocsComponent test instance
  h1 = fixture.nativeElement.querySelector('h1');
});

it('should display original title after detectChanges()', () => {
  fixture.detectChanges();
  // The way to do it without ComponentFixtureAutoDetect
  expect(h1.textContent).toContain(component.title);
});

//The first test shows the benefit of automatic change detection.
it('should display original title', () => {
  // Hooray! No `fixture.detectChanges()` needed
  expect(h1.textContent).toContain(component.title);
});

//The second and third test reveal an important limitation. The Angular testing environment does not know that the test changed the component's title.
it('should still see original title after comp.title change', () => {
  const oldTitle = component.title;
  component.title = 'Test Title';
  // Displayed title is old because Angular didn't hear the change :(
  expect(h1.textContent).toContain(oldTitle);
});

it('should display updated title after detectChanges', () => {
  component.title = 'Test Title';
  fixture.detectChanges(); // detect changes explicitly
  expect(h1.textContent).toContain(component.title);
});
