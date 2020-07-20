import { Component } from '@angular/core';
import { Athlete } from "./Athlete";

@Component({
  selector: 'app-root',
  template: `<h1>Pięciu najlepszych zawodników w Kona</h1>
  <app-athlete-list (selected)=showDetails($event)>Wczytywanie listy zawodników...</app-athlete-list>
  Wybrałeś: {{selectedAthlete}}`
})
export class AppComponent {
  selectedAthlete: string;

  constructor (){
    this.selectedAthlete="żaden";
  }

  showDetails(selectedAthlete: Athlete) {
    this.selectedAthlete=selectedAthlete.name;
  }
}
