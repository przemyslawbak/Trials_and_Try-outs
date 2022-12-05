import { Component, ViewChild, ElementRef } from "@angular/core";
import { AnimationBuilder } from "@angular/animations";
import { Vec2, Vec5 } from "./services/loader.service";

import { LoaderService } from "./services/loader.service";
import { Tween } from "./services/loader.service";
@Component({
  selector: "my-app",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.css"],
  viewProviders: [LoaderService]
})
export class AppComponent {
  @ViewChild("aniDiv", { static: false }) aniDiv: any;
  @ViewChild("aniDiv2", { static: false }) aniDiv2: any;
  name = "Angular";
  p = 0;
  animationArray: Array<any> = [];
  newurl =
    "https://www.spottedzebra.us/img/posts/2013-05-20-XAML%20Spritesheet%20Animation/scared-owl-spritesheet.png";
  url1 =
    "https://ragingbanana.files.wordpress.com/2015/03/35f7d4fa3fdf5ae46be107004f93497a.png";
  constructor(
    private loaderService: LoaderService,
    private animationBuilder: AnimationBuilder
  ) {
    const images = [
      "https://picsum.photos/id/237/200/300",
      "https://picsum.photos/seed/picsum/200/300",
      "https://picsum.photos/200/300?grayscale",
      "https://picsum.photos/200/300/?blur",
      "https://ragingbanana.files.wordpress.com/2015/03/35f7d4fa3fdf5ae46be107004f93497a.png",
      "https://www.spottedzebra.us/img/posts/2013-05-20-XAML%20Spritesheet%20Animation/scared-owl-spritesheet.png"
    ];
    loaderService.loadImages(images).then(r => {});
    loaderService.progressCallBack = p => {
      this.p = p;
    };
    Tween.setAnimationBuilder(animationBuilder);
    for (let i = 0; i < 10; i++) {
      const fps = 12 + Math.random() * 60;

      const obj = {
        id: i + 1,
        frameRate: Math.round(fps)
      };
      this.animationArray.push(obj);
    }
  }
  showMe(status: boolean) {
    console.log(status);
  }
  ngAfterViewInit() {}
  removeSelf(index) {
    this.animationArray.splice(index, 1);
  }

  removeTen() {
    if (this.animationArray.length < 10) {
      return alert("i don't like you");
    }
    for (let i = 0; i < 10; i++) {
      this.pop();
    }
  }
  ten() {
    for (let i = 0; i < 10; i++) {
      this.push();
    }
  }
  push() {
    const fps = 12 + Math.random() * 60;
    const obj = {
      id: this.animationArray.length + 1,
      frameRate: Math.round(fps)
    };
    this.animationArray.push(obj);
  }
  pop() {
    this.animationArray.pop();
  }
  shift() {
    this.animationArray.shift();
  }
  unshift() {
    this.animationArray.unshift();
  }
}
