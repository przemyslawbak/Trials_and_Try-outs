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

  drop(event: CdkDragDrop<Array<ShipComponent>>) {
    console.clear();
    console.log(this.position);
    console.log(
      "el x: " + this.boardElement.nativeElement.getBoundingClientRect().x
    );
    console.log(
      "el y: " + this.boardElement.nativeElement.getBoundingClientRect().y
    );
    event.previousContainer.data[event.previousIndex].top = this.position
      ? this.position.y -
        this.boardElement.nativeElement.getBoundingClientRect().y
      : 0;
    event.previousContainer.data[event.previousIndex].left = this.position
      ? this.position.x -
        this.boardElement.nativeElement.getBoundingClientRect().x
      : 0;

    console.log("t: " + event.previousContainer.data[event.previousIndex].top);
    console.log("l: " + event.previousContainer.data[event.previousIndex].left);

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
