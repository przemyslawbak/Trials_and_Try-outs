import {
  Component,
  Output,
  OnInit,
  Input,
  ElementRef,
  EventEmitter,
  ViewChild
} from "@angular/core";

@Component({
  selector: "app-animated-sprite",
  templateUrl: "./animated-sprite.component.html",
  styleUrls: ["./animated-sprite.component.css"]
})
export class AnimatedSpriteComponent implements OnInit {
  @ViewChild("animationRef", { static: true }) animationRef: ElementRef;
  constructor() {}
  //@required method must
  @Input() public id: number = 0;
  @Input() public url: string;
  @Input() public frameRate: number = 12;
  @Input() public totalRows: number = 8;
  @Input() public totalcols: number = 8;
  @Input() public totalFrames: number = 12;
  @Output() public animationFinish = new EventEmitter<boolean>();
  //current frame
  @Input() private currentFrame: number = 0;
  @Input() private isLoop: Boolean = true;
  //all elements
  private frameInterval = 0;
  private startTime = 0;
  private now = 0;
  private then = 0;
  private elapsed = 0;
  //all elements
  private size = { width: 0, height: 0 };
  public width: number = 5;
  public height: number = 5;
  private directionX = 0;
  private directionY = 0;
  private positionX = 0;
  private positionY = 0;
  private stop: boolean = false;
  ngOnInit() {
    //)
  }
  resetSprite() {
    this.currentFrame = 0;
    this.directionX = 0;
    this.directionY = 0;
    this.positionX = this.directionX * this.width;
    this.positionY = this.directionY * this.height;
    this.animationRef.nativeElement.style["background-position-x"] =
      this.positionX + "px";
    this.animationRef.nativeElement.style["background-position-y"] =
      this.positionY + "px";
  }
  updateSprite() {
    if (this.totalFrames <= this.currentFrame) {
      if (this.isLoop) {
        this.resetSprite();
      } else {
        this.stop = true;
        this.animationFinish.emit(true);
        return;
      }
    } else {
      this.currentFrame++;
    }

    //check for x direction

    if (Math.abs(this.positionY) > this.size.height) {
      this.directionX = 0;
      this.directionY = 0;
      this.positionX = this.directionX * this.width;
      this.positionY = this.directionY * this.height;
    } else if (Math.abs(this.positionX) > this.size.width) {
      this.directionX = 0;
      this.directionY -= 1;
      this.positionX = 0;
      this.positionY = this.directionY * this.height;
    } else {
      this.directionX -= 1;
      this.positionX = this.directionX * this.width;
    }
    this.animationRef.nativeElement.style["background-position-x"] =
      this.positionX + "px";
    this.animationRef.nativeElement.style["background-position-y"] =
      this.positionY + "px";
  }
  startAnimating(fps) {
    console.log("starting animation");
    this.frameInterval = 1000 / this.frameRate;
    this.then = Date.now();
    this.startTime = this.then;
    this.animate();
  }

  animate() {
    if (this.stop) {
      return;
    }
    requestAnimationFrame(() => {
      this.animate();
    });
    this.now = Date.now();
    this.elapsed = this.now - this.then;
    if (this.elapsed > this.frameInterval) {
      this.then = this.now - (this.elapsed % this.frameInterval);
      this.updateSprite();
    }
  }
  ngAfterViewInit() {
    const image = new Image();
    image.src = this.url;
    image.onload = () => {
      this.size = {
        width: image.naturalWidth,
        height: image.naturalHeight
      };
      //10, 6.7
      this.width = this.size.width / this.totalcols;
      this.height = this.size.height / this.totalRows;
      this.startAnimating(this.frameRate);
    };
  }
  getCssStyle() {
    const defaultStyle = "ani_style";
    return defaultStyle;
  }
}
