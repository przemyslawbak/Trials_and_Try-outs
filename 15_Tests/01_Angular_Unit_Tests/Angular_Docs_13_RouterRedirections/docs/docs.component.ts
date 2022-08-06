import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Hero } from './hero.model';
import { HeroService } from './hero.service';

@Component({
  selector: 'docs',
  templateUrl: './docs.component.html',
  styleUrls: ['./docs.component.css'],
})
export class DocsComponent implements OnInit {
  public selectedHero: Hero = {} as Hero;
  public heroes: Hero[] = [];
  constructor(private router: Router, private heroService: HeroService) {}

  ngOnInit(): void {}

  gotoDetail(hero: Hero) {
    const url = `/heroes/${hero.id}`;
    this.router.navigateByUrl(url);
  }
}
