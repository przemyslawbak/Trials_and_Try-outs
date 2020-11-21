import { Component, ElementRef, ViewChild } from "@angular/core";
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

  @ViewChild("board", { read: ElementRef, static: false }) boardElement: any;

  public dragEnded(event: CdkDragEnd): void {
    this.dragEnd = this.hoverPlace;
    this.increaseZIndex(event.source.element);

    if (this.dragEnd.type === "cell" && this.dragStart.type !== "cell") {
      this.moveFromList1To2(event.source.element.nativeElement.id);
      event.source._dragRef.reset();
    }

    if (this.dragEnd.type !== "cell" && this.dragStart.type !== "list") {
      this.moveFromList2To1(event.source.element.nativeElement.id);
      event.source._dragRef.reset();
    }

    if (this.dragEnd.type === "list" && this.dragStart.type === "list") {
      event.source._dragRef.reset();
    }

    if (this.dragEnd.type === "cell" && this.dragStart.type === "cell") {
      let index: number = +event.source.element.nativeElement.id;
      let item = this.list2[index];
      item = this.updateShipsCss(item);
      this.list2.splice(index, 1);
      this.list2.push(item);
      event.source._dragRef.reset();
    }
  }

  private moveFromList2To1(id: string): void {
    let index: number = +id;
    let item = this.list2[index];
    let temp = [item].concat(this.list1);
    this.list1 = temp;
    this.list2.splice(index, 1);
  }

  private moveFromList1To2(id: string): void {
    let index: number = +id;
    let item = this.updateShipsCss(this.list1[index]);
    this.list2.push(item);
    this.list1.splice(index, 1);
  }

  public updateOnBoardCss(ship: ShipComponent): ShipComponent {
    console.clear();
    console.log(this.dragEnd.cellX);
    console.log(this.dragEnd.cellY);
    ship.left = this.dragEnd.cellX;
    ship.top = this.dragEnd.cellY;
    return ship;
  }

  public updateShipsCss(ship: ShipComponent): ShipComponent {
    ship.left =
      this.dragEnd.cellX -
      this.boardElement.nativeElement.getBoundingClientRect().x;
    ship.top =
      this.dragEnd.cellY -
      this.boardElement.nativeElement.getBoundingClientRect().y;
    return ship;
  }

  public hoveredElement(
    position: any,
    elementType: string,
    row: number,
    col: number
  ): void {
    let dropPlace = {} as DragModel;
    dropPlace.cellX = position.x;
    dropPlace.cellY = position.y;
    dropPlace.type = elementType;
    dropPlace.row = row;
    dropPlace.col = col;
    this.hoverPlace = dropPlace;
  }

  public dragStarted(event: CdkDragStart): void {
    this.dragStart = this.hoverPlace;
  }

  public dragMoved(event: CdkDragMove): void {
    this.decreaseZIndex(event.source.element);
  }

  private decreaseZIndex(element: ElementRef): void {
    element.nativeElement.style.zIndex = "-1";
    let el = element.nativeElement.children[0] as HTMLElement;
    el.style.zIndex = "-1";
  }

  private increaseZIndex(element: ElementRef): void {
    element.nativeElement.style.zIndex = "100";
    let el = element.nativeElement.children[0] as HTMLElement;
    el.style.zIndex = "100";
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
