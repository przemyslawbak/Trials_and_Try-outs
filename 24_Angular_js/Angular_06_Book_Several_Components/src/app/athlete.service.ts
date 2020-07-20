import { Injectable } from '@angular/core';
import { Athlete } from './Athlete';

@Injectable()
export class AthleteService {
  getAthletes(){
    return [
    {name:"Jan Frodeno", country: "Niemcy", time: "08:06:30"},
    {name:"Sebastian Kienle", country: "Niemcy", time: "08:10:02"},
    {name:"Patrick Lange", country: "Niemcy", time: "08:11:14"},
    {name:"Ben Hoffman", country: "USA", time: "08:13:00"},
    {name:"Andi Boecherer", country: "Niemcy", time: "08:13:25"}
  ];
  }
}