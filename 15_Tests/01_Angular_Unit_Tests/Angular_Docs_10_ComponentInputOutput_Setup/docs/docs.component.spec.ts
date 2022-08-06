import { DebugElement } from '@angular/core';
import { TestBed, ComponentFixture } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DashboardHeroComponent } from './dashboard-hero.component';
import { Hero } from './hero.model';

let heroDe: DebugElement;
let heroEl: HTMLElement;
let fixture: ComponentFixture<DashboardHeroComponent>;
let component: DashboardHeroComponent;
let expectedHero: Hero;

beforeEach(() => {
  TestBed.configureTestingModule({ declarations: [DashboardHeroComponent] });
  fixture = TestBed.createComponent(DashboardHeroComponent);
  component = fixture.componentInstance;

  // find the hero's DebugElement and element
  heroDe = fixture.debugElement.query(By.css('.hero'));
  heroEl = heroDe.nativeElement;

  // mock the hero supplied by the parent component
  expectedHero = { id: 42, name: 'Test Name' };

  // simulate the parent setting the input property with that hero
  component.hero = expectedHero;

  // trigger initial data binding
  fixture.detectChanges();
});

it('should display hero name in uppercase', () => {
  const expectedPipedName = expectedHero.name.toUpperCase();
  expect(heroEl.textContent).toContain(expectedPipedName);
});
