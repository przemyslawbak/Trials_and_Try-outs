import { Component, Input } from '@angular/core';
import { Athlete } from './Athlete';

@Component({
  selector: 'app-athlete',
  template: `{{athlete.name}} ({{athlete.country}}): {{athlete.time}}`
})
export class AthleteComponent {
  @Input() athlete: Athlete;
  constructor() { }
}
