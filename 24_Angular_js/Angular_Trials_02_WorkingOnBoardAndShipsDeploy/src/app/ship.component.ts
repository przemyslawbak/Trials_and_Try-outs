import { Component, Input } from "@angular/core";

@Component({
  selector: "app-ship",
  templateUrl: "./ship.component.html",
  styleUrls: ["./ship.component.css"],
})
export class ShipComponent {
  @Input() size: number;
  @Input() rotate: boolean;

  constructor() {
    this.size = 0;
    this.rotate = false;
  }
}
