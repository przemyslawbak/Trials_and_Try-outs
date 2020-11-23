import { Component, ElementRef, ViewChild } from "@angular/core";
import { CdkDragEnd } from "@angular/cdk/drag-drop";
import { ShipComponent } from "./ship.component";
import { DragModel } from "./drop.model";
import { BoardCellModel } from "./board-cell.model";

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.css"],
})
export class AppComponent {
  public list1: Array<ShipComponent>;
  public list2: Array<ShipComponent>;
  public boardP1: BoardCellModel[][];
  public hoverPlace: DragModel = {} as DragModel;
  public dragStart: DragModel = {} as DragModel;
  public dragEnd: DragModel = {} as DragModel;

  constructor() {
    this.boardP1 = this.getEmptyBoard();
    this.list1 = this.createFleet();
    this.list2 = [];
  }

  @ViewChild("board", { read: ElementRef, static: false }) boardElement: any;
  @ViewChild("nextItem", { read: ElementRef, static: false }) nextItem: any;

  public setRotation(name: string): void {
    const id: string = this.getIdFromElementName(name);
    let item: ShipComponent = this.getArrayItem(name, id);
    item.rotation = item.rotation === 0 ? 90 : 0;
  }

  public deployShip(): void {
    //todo: on board cell click
  }

  public resetElement(element: HTMLElement) {
    element.style.backgroundColor = "rgba(0, 162, 255, 0.2)";
  }

  public hoveredElement(
    position: any,
    elementType: string,
    row: number,
    col: number,
    element: HTMLElement
  ): void {
    let dropPlace = {} as DragModel;
    dropPlace.cellX = position.x;
    dropPlace.cellY = position.y;
    dropPlace.type = elementType;
    dropPlace.row = row;
    dropPlace.col = col;

    let dropCells: Array<BoardCellModel> = this.getDropCells(row, col);

    if (elementType == "cell") {
      let isDropAllowed: boolean = this.validateDropPlace(dropCells);

      if (isDropAllowed) {
        this.hoverPlace = dropPlace;
        //todo: get cells to color from: dropCells
        element.style.backgroundColor = "rgb(0, 162, 255)";
      } else {
        //todo: colour cell red
        element.style.backgroundColor = "red";
      }
    }
  }

  private updateShipsCss(ship: ShipComponent): ShipComponent {
    ship.left =
      this.dragEnd.cellX -
      this.boardElement.nativeElement.getBoundingClientRect().x;
    ship.top =
      this.dragEnd.cellY -
      this.boardElement.nativeElement.getBoundingClientRect().y;
    return ship;
  }

  private moveFromList2To1(id: string): void {
    const index: number = +id;
    const item = this.list2[index];
    const temp = [item].concat(this.list1);
    this.list1 = temp;
    this.list2.splice(index, 1);
  }

  private moveFromList1To2(id: string): void {
    const index: number = +id;
    const item = this.updateShipsCss(this.list1[index]);
    this.list2.push(item);
    this.list1.splice(index, 1);
  }

  private createFleet(): Array<ShipComponent> {
    return [
      { size: 4, top: 0, left: 0, deployed: false, rotation: 0 },
      { size: 3, top: 0, left: 0, deployed: false, rotation: 0 },
      { size: 3, top: 0, left: 0, deployed: false, rotation: 0 },
      { size: 2, top: 0, left: 0, deployed: false, rotation: 0 },
      { size: 2, top: 0, left: 0, deployed: false, rotation: 0 },
      { size: 2, top: 0, left: 0, deployed: false, rotation: 0 },
      { size: 1, top: 0, left: 0, deployed: false, rotation: 0 },
      { size: 1, top: 0, left: 0, deployed: false, rotation: 0 },
      { size: 1, top: 0, left: 0, deployed: false, rotation: 0 },
      { size: 1, top: 0, left: 0, deployed: false, rotation: 0 },
    ];
  }

  private getEmptyBoard(): BoardCellModel[][] {
    let board: BoardCellModel[][] = [];
    for (let i = 0; i < 10; i++) {
      board[i] = [];
      for (let j = 0; j < 10; j++) {
        board[i][j] = { row: j, col: i, value: 0 } as BoardCellModel;
      }
    }

    return board;
  }

  private getArrayItem(name: string, id: string): ShipComponent {
    return name.split("-")[1] === "list1" ? this.list1[+id] : this.list2[+id];
  }

  private getIdFromElementName(name: string): string {
    return name.split("-")[0];
  }

  private getListFromName(name: string): Array<ShipComponent> {
    return name.split("-")[1] === "list1" ? this.list1 : this.list2;
  }

  private validateDropPlace(dropPlace: Array<BoardCellModel>): boolean {
    let result: boolean = true;

    result = dropPlace.length != this.list1[0].size ? false : true;

    result = this.isShipTouchingOther(dropPlace) ? false : true;

    return result;
  }

  private isShipTouchingOther(dropPlace: BoardCellModel[]): boolean {
    //todo: avoid ship deploy touching corners
    //todo: avoid ship deploy touching sides
    //todo: avoid ship deploy on top of eachother
    throw new Error("Method not implemented.");
  }

  private getDropCells(row: number, col: number): Array<BoardCellModel> {
    let result: Array<BoardCellModel> = [];
    for (let i = 0; i < this.list1[0].size; i++) {
      let cellModel: BoardCellModel = this.getCell(
        this.list1[0].rotation,
        i,
        row,
        col
      );

      if (cellModel.col >= 0 && cellModel.row >= 0 && cellModel.value >= 0) {
        result.push(cellModel);
      }
    }

    return result;
  }

  private getCell(
    rotation: number,
    i: number,
    row: number,
    col: number
  ): BoardCellModel {
    let item: BoardCellModel =
      rotation == 0
        ? ({ row: row, col: col + i, value: 0 } as BoardCellModel)
        : ({ row: row + i, col: col, value: 0 } as BoardCellModel);

    let exists = this.boardP1.some((b) =>
      b.some((c) => c.row == item.row && c.col == item.col)
    );

    return exists ? item : ({ row: -1, col: -1, value: -1 } as BoardCellModel);
  }

  private deployEnded(event: CdkDragEnd): void {
    this.dragEnd = this.hoverPlace;

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
      const index: number = +event.source.element.nativeElement.id;
      let item = this.list2[index];
      item = this.updateShipsCss(item);
      this.list2.splice(index, 1);
      this.list2.push(item);
      event.source._dragRef.reset();
    }
  }
}
