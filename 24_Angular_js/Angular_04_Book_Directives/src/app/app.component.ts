import { Component } from '@angular/core';
@Component({
selector: 'app-root',
template: `
<h1>Pięciu najlepszych zawodników w Kona</h1>
<ol>
<li *ngFor="let athlete of athletes">{{athlete.name}}
({{athlete.country}}):{{athlete.time}}</li>
</ol>
`
})
export class AppComponent {
athletes = [
{name:"Jan Frodeno", country: "Niemcy", time: "08:06:30"},
{name:"Sebastian Kienle", country: "Niemcy", time: "08:10:02"},
{name:"Patrick Lange", country: "Niemcy", time: "08:11:14"},
{name:"Ben Hoffman", country: "USA", time: "08:13:00"},
{name:"Andi Boecherer", country: "Niemcy", time: "08:13:25"}
];
}