import { element } from "protractor";
import { Component } from "@angular/core";
import { CdkDragEnd, CdkDragMove, CdkDragStart } from "@angular/cdk/drag-drop";
import { ShipComponent } from "./ship.component";
import { DragModel } from "./drop.model";

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.css"],
})
export class AppComponent {
  public list1: Array<ShipComponent>;
  public list2: Array<ShipComponent>;
  public boardP1: number[][];
  public hoverPlace: DragModel = {} as DragModel;
  public dragStart: DragModel = {} as DragModel;
  public dragEnd: DragModel = {} as DragModel;
  public xInitial: number = 0;
  public yInitial: number = 0;

  constructor() {
    this.boardP1 = this.getEmptyBoard();
    this.list1 = this.createFleet();
    this.list2 = [];
  }

  public dragEnded(event: CdkDragEnd) {
    console.clear();

    console.log(event.source.element.nativeElement.style.top);
    this.dragEnd = this.hoverPlace;

    if (this.dragEnd.type === "cell" && this.dragStart.type !== "cell") {
      this.moveFromList1To2(event.source.element.nativeElement.id);
      event.source._dragRef.reset();
    }
  }

  private moveFromList1To2(id: string) {
    let index: number = +id;
    let item = this.updateShip(this.list1[index]);
    this.list2.push(item);
    this.list1.splice(index, 1);
    console.log("1: " + this.list1.length);
    console.log("1: " + this.list2.length);
  }

  private updateShip(ship: ShipComponent): ShipComponent {
    ship.left = this.dragEnd.cellX;
    ship.top = this.dragEnd.cellY;
    console.log(this.dragEnd.cellX);
    console.log(this.dragEnd.cellY);

    return ship;
  }

  public hoveredElement(
    position: any,
    elementType: string,
    row: number,
    col: number
  ) {
    let dropPlace = {} as DragModel;
    dropPlace.cellX = position.x;
    dropPlace.cellY = position.y;
    dropPlace.type = elementType;
    dropPlace.row = row;
    dropPlace.col = col;
    this.hoverPlace = dropPlace;
  }

    public dragStarted(event: CdkDragStart) {
    this.dragStart = this.hoverPlace;
    console.log(this.dragStart.cellY);
    console.log(this.dragStart.cellX);
  }

    public dragMoved(event: CdkDragMove) {
    //
  }

  private createFleet(): Array<ShipComponent> {
    return [
      { size: 4, rotate: false, top: 0, left: 0, deployed: false },
      { size: 3, rotate: false, top: 0, left: 0, deployed: false },
      { size: 3, rotate: false, top: 0, left: 0, deployed: false },
      { size: 2, rotate: false, top: 0, left: 0, deployed: false },
      { size: 2, rotate: false, top: 0, left: 0, deployed: false },
      { size: 2, rotate: false, top: 0, left: 0, deployed: false },
      { size: 1, rotate: false, top: 0, left: 0, deployed: false },
      { size: 1, rotate: false, top: 0, left: 0, deployed: false },
      { size: 1, rotate: false, top: 0, left: 0, deployed: false },
      { size: 1, rotate: false, top: 0, left: 0, deployed: false },
    ];
  }

  private getEmptyBoard(): number[][] {
    return Array.from({ length: 10 }, () => Array(10).fill(0));
  }
}
