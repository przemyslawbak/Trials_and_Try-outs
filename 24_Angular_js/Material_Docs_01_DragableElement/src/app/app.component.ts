import { AfterViewInit, Component, ElementRef, ViewChild } from "@angular/core";
import { DragDrop } from "@angular/cdk/drag-drop";

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.css"],
})
export class AppComponent implements AfterViewInit {
  @ViewChild("dragable") dragable: any;

  constructor(private dragDropService: DragDrop) {}

  public ngAfterViewInit() {
    this.dragDropService.createDrag(this.dragable);
  }
}
