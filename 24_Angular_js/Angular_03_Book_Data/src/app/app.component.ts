import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  //template: `<h1 [style.color]="color">{{attached}}</h1>
//<button (click)="setColor()">Zmień kolor</button>`,
template: `
<h1>{{twoway}}</h1>
<input type="text" [(ngModel)]="twoway" />
`
})
export class AppComponent {
twoway = 'Przykład dwukierunkowego dołączania danych';
  title = 'interpolacja';
  something = 'jednokierunkowe dołączenie danych';
  attached = 'dołączanie zdarzenia';
  color = "";
  setColor(){
if(this.color==="")
this.color="red";
else
this.color="";
};
}

