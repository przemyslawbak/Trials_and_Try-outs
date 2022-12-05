import { Component } from '@angular/core';
import { CdkDragDrop, CdkDragEnd } from '@angular/cdk/drag-drop';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  public boardP1: number[][];
  public player1Fleet: boolean[][];

  constructor() {
    this.boardP1 = this.getEmptyBoard();
    this.player1Fleet = this.createFleet();
  }

  dragEnd($event: CdkDragEnd) {
    console.log($event.source.getFreeDragPosition());
  }

  onDrop() {
    console.log('drop');
  }

  private createFleet(): boolean[][] {
    return [
      [false],
      [false],
      [false],
      [false],
      [false, false],
      [false, false],
      [false, false],
      [false, false, false],
      [false, false, false],
      [false, false, false, false],
    ];
  }

  private getEmptyBoard(): number[][] {
    return Array.from({ length: 10 }, () => Array(10).fill(0));
  }
}
