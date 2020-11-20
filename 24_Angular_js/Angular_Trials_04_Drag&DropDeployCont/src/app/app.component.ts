import { Component, ElementRef, ViewChild } from "@angular/core";
import {
  CdkDragDrop,
  moveItemInArray,
  transferArrayItem,
} from "@angular/cdk/drag-drop";
import { ShipComponent } from "./ship.component";

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.css"],
})

//https://stackblitz.com/edit/angular-salbpl?file=src%2Fapp%2Fapp.component.html
export class AppComponent {
  public list1: Array<ShipComponent>;
  public list2: Array<ShipComponent>;
  public boardP1: number[][];
  public position: any;

  constructor() {
    this.boardP1 = this.getEmptyBoard();
    this.list1 = this.createFleet();
    this.list2 = [];
  }

  @ViewChild("two", { read: ElementRef, static: false }) boardElement: any;
  @ViewChild("ships", { read: ElementRef, static: false }) shipsElement: any;

  onDrop(event: CdkDragDrop<Array<ShipComponent>>) {
    console.clear();
    console.log(this.position);
    event.previousContainer.data[event.previousIndex].top =
      this.position.y -
      this.boardElement.nativeElement.getBoundingClientRect().y;
    event.previousContainer.data[event.previousIndex].left =
      this.position.x -
      this.boardElement.nativeElement.getBoundingClientRect().x;

    if (event.previousContainer === event.container) {
      moveItemInArray(
        event.container.data,
        event.previousIndex,
        event.currentIndex
      );
    } else {
      transferArrayItem(
        event.previousContainer.data,
        event.container.data,
        event.previousIndex,
        event.currentIndex
      );
    }

    console.log("t: " + event.container.data[event.previousIndex].top);
    console.log("l: " + event.container.data[event.previousIndex].left);
    console.log("s: " + event.container.data[event.previousIndex].size);
    if (this.shipsElement.nativeElement) {
      console.log("top: " + this.shipsElement.nativeElement.style.top);
      console.log("left: " + this.shipsElement.nativeElement.style.left);
    }
  }

  private createFleet(): Array<ShipComponent> {
    return [
      { size: 4, rotate: false, top: 0, left: 0 },
      { size: 3, rotate: false, top: 0, left: 0 },
      { size: 3, rotate: false, top: 0, left: 0 },
      { size: 2, rotate: false, top: 0, left: 0 },
      { size: 2, rotate: false, top: 0, left: 0 },
      { size: 2, rotate: false, top: 0, left: 0 },
      { size: 1, rotate: false, top: 0, left: 0 },
      { size: 1, rotate: false, top: 0, left: 0 },
      { size: 1, rotate: false, top: 0, left: 0 },
      { size: 1, rotate: false, top: 0, left: 0 },
    ];
  }

  private getEmptyBoard(): number[][] {
    return Array.from({ length: 10 }, () => Array(10).fill(0));
  }
}
