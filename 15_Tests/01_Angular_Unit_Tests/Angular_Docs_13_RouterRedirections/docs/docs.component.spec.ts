import { Component } from '@angular/core';
import { TestBed, ComponentFixture } from '@angular/core/testing';
import { Router } from '@angular/router';
import { DocsComponent } from './docs.component';
import { HeroService } from './hero.service';

const routerSpy = jasmine.createSpyObj('Router', ['navigateByUrl']);
const heroServiceSpy = jasmine.createSpyObj('HeroService', ['getHeroes']);
let component: DocsComponent;
let fixture: ComponentFixture<DocsComponent>;

beforeEach(() => {
  TestBed.configureTestingModule({
    providers: [
      { provide: HeroService, useValue: heroServiceSpy },
      { provide: Router, useValue: routerSpy },
    ],
  });
  component = fixture.componentInstance;
});

it('should tell ROUTER to navigate when hero clicked', () => {
  //something is missing in SUT...
  heroClick(); // trigger click on first inner <div class="hero">

  // args passed to router.navigateByUrl() spy
  const spy = routerSpy.navigateByUrl as jasmine.Spy;
  const navArgs = spy.calls.first().args[0];

  // expecting to navigate to id of the component's first hero
  const id = component.heroes[0].id;
  expect(navArgs).toBe(
    '/heroes/' + id,
    'should nav to HeroDetail for first hero'
  );
});
