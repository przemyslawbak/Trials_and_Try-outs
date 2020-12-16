import { Component, ViewChild } from "@angular/core";
import { AnimatedSpriteComponent } from "./animated-sprite/animated-sprite.component";
@Component({
  selector: "my-app",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.css"],
  viewProviders: []
})
export class AppComponent {
  @ViewChild("aniDiv", { static: false }) aniDiv: any;
  @ViewChild("aniDiv2", { static: false }) aniDiv2: any;
  public display: boolean = false;
  name = "Angular";
  posX: number = 0;
  posY: number = 0;
  p = 0;
  animationArray: Array<any> = [];
  newurl =
    "https://www.spottedzebra.us/img/posts/2013-05-20-XAML%20Spritesheet%20Animation/scared-owl-spritesheet.png";
  constructor() {}

  //add animation on click
  public onClick(): void {
    this.display = !this.display;
    const obj = {
      id: 1,
      frameRate: 10
    };
    this.posX = this.getRandomX();
    this.posY = this.getRandomY();
    this.animationArray.push(obj);
  }

  private getRandomY(): number {
    return Math.floor(Math.random() * 100);
  }

  private getRandomX(): number {
    return Math.floor(Math.random() * 100);
  }
}
