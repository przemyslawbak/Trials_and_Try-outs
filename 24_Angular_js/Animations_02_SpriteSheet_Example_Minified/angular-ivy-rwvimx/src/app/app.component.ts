import { Component, ViewChild } from "@angular/core";
@Component({
  selector: "my-app",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.css"],
  viewProviders: []
})
export class AppComponent {
  @ViewChild("aniDiv", { static: false }) aniDiv: any;
  @ViewChild("aniDiv2", { static: false }) aniDiv2: any;
  name = "Angular";
  p = 0;
  animationArray: Array<any> = [];
  newurl =
    "https://www.spottedzebra.us/img/posts/2013-05-20-XAML%20Spritesheet%20Animation/scared-owl-spritesheet.png";
  constructor() {
    //animating
    const fps = 12 + Math.random() * 60;

    const obj = {
      id: 0 + 1,
      frameRate: Math.round(fps)
    };
    this.animationArray.push(obj);
  }
}
