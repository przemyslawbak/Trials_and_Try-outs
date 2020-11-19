import { Component, ElementRef, ViewChild } from "@angular/core";
import {
  CdkDragDrop,
  CdkDropList,
  moveItemInArray,
  transferArrayItem,
} from "@angular/cdk/drag-drop";
import { ShipComponent } from "./ship.component";

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.css"],
})

//based on: https://stackblitz.com/edit/angular-salbpl?file=src%2Fapp%2Fapp.component.ts
//https://stackoverflow.com/questions/59916765/drag-and-drop-in-angular-on-complex-board-matrix#_=_
export class AppComponent {
  private fleetList: any;
  private boardList: any;
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

  drop(event: CdkDragDrop<any[]>) {
    event.previousContainer.data[event.previousIndex].top = this.position
      ? this.position.y -
        this.boardElement.nativeElement.getBoundingClientRect().y
      : 0;
    event.previousContainer.data[event.previousIndex].left = this.position
      ? this.position.x -
        this.boardElement.nativeElement.getBoundingClientRect().x
      : 0;
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
}
