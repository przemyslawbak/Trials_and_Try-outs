import { Component, Input } from "@angular/core";

@Component({
  selector: "app-ship",
  template: `
    <div class="ship-box" [style.width]="52 * size + 'px'">
      {{ name }}
      <div class="ship-drop-wrapper">
        <div
          *ngFor="let i of [0, 1, 2, 3, 4].slice(0, size)"
          class="ship-box-cell"
          (mouseover)="index = i"
        ></div>
      </div>
    </div>
  `,

  styles: [
    `
      .ship-box {
        position: relative;
        height: 50px;
        z-index: 100;
      }
      .ship-drop-wrapper {
        position: absolute;
        top: 0;
      }
      .ship-box-cell {
        width: 50px;
        height: 50px;
        border: 1px solid;
        display: inline-block;
        background-color: transparent;
      }
    `,
  ],
})
export class ShipComponent {
  @Input() name: string;
  @Input() size: number;

  constructor() {
    this.name = "";
    this.size = 0;
  }

  index: number = -1;
}
