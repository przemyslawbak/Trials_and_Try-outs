import { Component, Output, EventEmitter } from '@angular/core';
import { AthleteService } from './athlete.service';
import { Athlete } from "./athlete";

@Component({
  selector: 'app-athlete-list',
  template: `
  <ol>
    <li *ngFor="let athlete of athletes">
      <app-athlete (click)="select(athlete)" [athlete]="athlete">
    </app-athlete></li>
  </ol>
  `,
})
export class AthleteListComponent {
  athletes: Array<Athlete>;
  @Output() selected = new EventEmitter<Athlete>();
  constructor(private athleteService: AthleteService){
    this.athletes=athleteService.getAthletes();
  }

  select(selectedAthlete: Athlete){
    this.selected.emit(selectedAthlete);
  }
}