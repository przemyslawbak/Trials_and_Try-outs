import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Hero } from './hero.model';

@Component({
  selector: 'docs',
  template: ` <h2 highlight="yellow">Something Yellow</h2>
    <h2 highlight>The Default (Gray)</h2>
    <h2>No Highlight</h2>
    <input #box [highlight]="box.value" value="cyan" />`,
  styleUrls: ['./docs.component.css'],
})
export class DocsComponent implements OnInit {
  public selectedHero: Hero = {} as Hero;
  public heroes: Hero[] = [];
  constructor(private router: Router) {}

  ngOnInit(): void {}

  gotoDetail(hero: Hero) {
    const url = `/heroes/${hero.id}`;
    this.router.navigateByUrl(url);
  }
}
