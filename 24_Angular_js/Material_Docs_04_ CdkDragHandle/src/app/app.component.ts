import { Component } from "@angular/core";
import { CdkDragStart } from "@angular/cdk/drag-drop";

@Component({
  selector: "my-app",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.css"],
})
export class AppComponent {
  name = "Angular";

  public dragging: boolean;

  constructor() {
    this.dragging = false;
  }

  public handleDragStart(event: CdkDragStart): void {
    this.dragging = true;
  }

  public handleClick(event: MouseEvent): void {
    if (this.dragging) {
      this.dragging = false;
      return;
    }
    alert("clicked!");
  }
}
