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

it('should raise selected event when clicked (triggerEventHandler)', () => {
  let selectedHero: Hero = {} as Hero;
  component.selected.subscribe((hero: Hero) => (selectedHero = hero));

  heroDe.triggerEventHandler('click', null);
  expect(selectedHero).toBe(expectedHero);
});
