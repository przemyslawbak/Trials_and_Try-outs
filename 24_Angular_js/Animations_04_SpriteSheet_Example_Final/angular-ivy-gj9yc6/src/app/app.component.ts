import { Component, ViewChild } from "@angular/core";
@Component({
  selector: "my-app",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.css"],
  viewProviders: []
})
export class AppComponent {
  //@ViewChild("aniDiv2", { static: false }) aniDiv2: any;
  public display: boolean = false;
  public posX: number = 0;
  public posY: number = 0;
  public animationArray: Array<any> = [];
  public newurl = "https://i.ibb.co/c3WLWN1/splash.png";
  constructor() {}

  //add animation on click
  public onClick(): void {
    this.display = !this.display;
    const obj = {
      id: 1,
      frameRate: 17
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

  public getNotification(evt: boolean): void {
    if (evt) {
      this.display = !this.display;
      this.animationArray = [];
    }
  }
}
