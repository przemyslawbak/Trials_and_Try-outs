import { Component, ElementRef, ViewChild } from "@angular/core";
import {
  CdkDragDrop,
  moveItemInArray,
  transferArrayItem,
} from "@angular/cdk/drag-drop";
@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.css"],
})

//based on: https://stackblitz.com/edit/angular-salbpl?file=src%2Fapp%2Fapp.component.ts
export class AppComponent {
  public boardP1: number[][];
  public player1Fleet: any[];
  public shipsInBoard: any[] = [];
  public position: any;

  constructor() {
    this.boardP1 = this.getEmptyBoard();
    this.player1Fleet = this.createFleet();
  }

  @ViewChild("cdkBoard", { read: ElementRef, static: false })
  boardElement: any;
  public index: number = -1;

  drop(event: CdkDragDrop<boolean[][]>) {
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

  private createFleet(): any {
    return [
      { name: "Submarin", size: 1 },
      { name: "frigate", size: 2 },
      { name: "destroyer", size: 3 },
      { name: "cruiser", size: 4 },
    ];
  }

  private getEmptyBoard(): number[][] {
    return Array.from({ length: 10 }, () => Array(10).fill(0));
  }
}
