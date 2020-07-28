import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { City } from './city';
@Component({
  selector: 'app-cities',
  templateUrl: './cities.component.html',
  styleUrls: ['./cities.component.css']
})
export class CitiesComponent {
  public cities: City[];
  constructor(private http: HttpClient) {
  }
  ngOnInit() {
    console.log();
    this.http.get<City[]>('http://localhost:53242/api/Cities/1').subscribe(result => {
      this.cities = result;
      console.log(this.cities[0]);
      }, error => console.error(error));
  }
}
