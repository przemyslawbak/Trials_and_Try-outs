import { Component, ElementRef, ViewChild } from "@angular/core";
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
  public isDropAllowed: boolean = false;
  public lastDropCells: Array<BoardCellModel> = [];

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

  public deployShip(row: number, col: number): void {
    if (this.isDropAllowed) {
      let dropCells: Array<BoardCellModel> = this.getDropCells(row, col);
      this.dragEnd = this.hoverPlace;
      this.moveFromList1To2();
      for (let i = 0; i < dropCells.length; i++) {
        this.boardP1[dropCells[i].col][dropCells[i].row].value = 1;
      }
    }
  }

  public resetElement(element: HTMLElement) {
    element.style.backgroundColor = "rgba(0, 162, 255, 0.2)";
    for (let i = 0; i < this.lastDropCells.length; i++) {
      this.boardP1[this.lastDropCells[i].col][this.lastDropCells[i].row].color =
        "rgba(0, 162, 255, 0.2)";
    }
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
    this.lastDropCells = dropCells;

    if (elementType == "cell") {
      let allow: boolean = this.validateDropPlace(dropCells);

      if (allow) {
        this.isDropAllowed = true;
        this.hoverPlace = dropPlace;
        for (let i = 0; i < dropCells.length; i++) {
          this.boardP1[dropCells[i].col][dropCells[i].row].color =
            "rgb(0, 162, 255)";
        }
      } else {
        this.isDropAllowed = false;
        element.style.backgroundColor = "red";
      }
    }
  }

  private moveFromList1To2(): void {
    const item = this.updateShipsTopLeft(this.list1[0]);
    this.list2.push(item);
    this.list1.splice(0, 1);
  }

  private updateShipsTopLeft(ship: ShipComponent): ShipComponent {
    ship.left =
      this.dragEnd.cellX -
      this.boardElement.nativeElement.getBoundingClientRect().x;
    ship.top =
      this.dragEnd.cellY -
      this.boardElement.nativeElement.getBoundingClientRect().y;
    return ship;
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
        board[i][j] = {
          row: j,
          col: i,
          value: 0,
          color: "rgba(0, 162, 255, 0.2)",
        } as BoardCellModel;
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

  private validateDropPlace(dropPlace: Array<BoardCellModel>): boolean {
    let result: boolean = true;

    if (dropPlace.length !== this.list1[0].size) {
      result = false;
    }
    if (!this.isShipNotTouchingOther(dropPlace)) {
      result = false;
    }

    return result;
  }

  private isShipNotTouchingOther(dropPlace: BoardCellModel[]): boolean {
    let result: boolean = true;
    let forbiddenCells: Array<BoardCellModel> = this.GetForbiddenCells(
      dropPlace
    );

    if (!this.compareBoardWithForbiddenCells(forbiddenCells)) {
      result = false;
    }

    return result;
  }

  private compareBoardWithForbiddenCells(
    forbiddenCells: Array<BoardCellModel>
  ): boolean {
    for (let i = 0; i < 10; i++) {
      for (let j = 0; j < 10; j++) {
        for (let f = 0; f < forbiddenCells.length; f++) {
          if (
            this.boardP1[i][j].col == forbiddenCells[f].col &&
            this.boardP1[i][j].row == forbiddenCells[f].row &&
            this.boardP1[i][j].value == 1
          ) {
            return false;
          }
        }
      }
    }

    return true;
  }

  private GetForbiddenCells(dropPlace: BoardCellModel[]): BoardCellModel[] {
    let list: BoardCellModel[] = [];

    for (let i = 0; i < dropPlace.length; i++) {
      list.push({
        row: dropPlace[i].row,
        col: dropPlace[i].col,
        value: 0,
      } as BoardCellModel);
      list.push({
        row: dropPlace[i].row,
        col: dropPlace[i].col + 1,
        value: 0,
      } as BoardCellModel);
      list.push({
        row: dropPlace[i].row + 1,
        col: dropPlace[i].col + 1,
        value: 0,
      } as BoardCellModel);
      list.push({
        row: dropPlace[i].row + 1,
        col: dropPlace[i].col,
        value: 0,
      } as BoardCellModel);
      list.push({
        row: dropPlace[i].row,
        col: dropPlace[i].col - 1,
        value: 0,
      } as BoardCellModel);
      list.push({
        row: dropPlace[i].row - 1,
        col: dropPlace[i].col - 1,
        value: 0,
      } as BoardCellModel);
      list.push({
        row: dropPlace[i].row - 1,
        col: dropPlace[i].col,
        value: 0,
      } as BoardCellModel);
      list.push({
        row: dropPlace[i].row - 1,
        col: dropPlace[i].col + 1,
        value: 0,
      } as BoardCellModel);
      list.push({
        row: dropPlace[i].row + 1,
        col: dropPlace[i].col - 1,
        value: 0,
      } as BoardCellModel);
    }

    return list;
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
}
