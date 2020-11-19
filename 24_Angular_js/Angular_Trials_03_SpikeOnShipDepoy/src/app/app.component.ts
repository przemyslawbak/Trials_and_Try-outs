import { Component } from "@angular/core";
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

  private createFleet(): Array<ShipComponent> {
    return [
      { size: 4, rotate: false },
      { size: 3, rotate: false },
      { size: 3, rotate: false },
      { size: 2, rotate: false },
      { size: 2, rotate: false },
      { size: 2, rotate: false },
      { size: 1, rotate: false },
      { size: 1, rotate: false },
      { size: 1, rotate: false },
      { size: 1, rotate: false },
    ];
  }

  private getEmptyBoard(): number[][] {
    return Array.from({ length: 10 }, () => Array(10).fill(0));
  }

  drop(event: CdkDragDrop<any>) {
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
}
