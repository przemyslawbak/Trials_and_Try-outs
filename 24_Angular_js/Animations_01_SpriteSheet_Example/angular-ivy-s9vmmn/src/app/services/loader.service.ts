import { Injectable } from "@angular/core";
import { CallbackFunctionVariadic } from "../callBacks";
import {
  trigger,
  state,
  style,
  animate,
  transition
} from "@angular/animations";
import { AnimationBuilder } from "@angular/animations";
export interface Vec2 {
  x: number;
  y: number;
}
export interface Vec3 {
  x: number;
  y: number;
  deg: number;
}
export interface Vec4 {
  x: number;
  y: number;
  deg: number;
  scale: number;
}
export interface Vec5 {
  position?: { x: number; y: number };
  deg?: number;
  scale?: { x: number; y: number };
  skew?: { x: number; y: number };
  time?: number;
}

//helper method can be found https://developer.mozilla.org/en-US/docs/Web/CSS/transform
export class Tween {
  static animationFinish: CallbackFunctionVariadic;
  static animationBuilder: AnimationBuilder;
  static allAnimation: Map<string, any> = new Map();

  //function
  static setAnimationBuilder(_animationBuilder: AnimationBuilder) {
    this.animationBuilder = _animationBuilder;
    this.allAnimation.set(
      "translate",
      animate(0, style({ transform: "translate(" + 0 + " px, " + 0 + " px)" }))
    ),
      this.allAnimation.set(
        "rotate",
        animate(0, style({ transform: "rotate(" + 0 + " deg)" }))
      ),
      this.allAnimation.set(
        "turn",
        animate(0, style({ transform: "rotate(" + 0 + "deg)" }))
      ),
      this.allAnimation.set(
        "scale",
        animate(0, style({ transform: "scale(" + 0 + ", " + 0 + ")" }))
      ),
      this.allAnimation.set(
        "skew",
        animate(0, style({ transform: "skew(" + 0 + "deg," + 0 + "deg)" }))
      );
  }
  //chain multiple animation
  static runSequence(
    ele: HTMLElement,
    sequence: Map<string, Vec5>,
    animationFinish: CallbackFunctionVariadic = undefined
  ) {
    let animationStack = [];
    for (let entry of Array.from(sequence.entries())) {
      const key = entry[0];
      const value = entry[1];
      let animation = this.allAnimation.get(key);
      const updateValue: Vec5 = {
        //change position
        position: {
          x: value.position ? value.position.x : 0,
          y: value.position ? value.position.y : 0
        },
        //rotate
        deg: value.deg ? value.deg : 0,
        //scale
        scale: {
          x: value.scale ? value.scale.x : 1,
          y: value.scale ? value.scale.y : 1
        },
        //skew
        skew: {
          x: value.skew ? value.skew.x : 0,
          y: value.skew ? value.skew.y : 0
        }
      };
      animation.styles.styles["transform"] =
        "rotate(" +
        updateValue.deg +
        "deg)" +
        "translate(" +
        updateValue.position.x +
        "px," +
        updateValue.position.y +
        "px)" +
        "scale(" +
        updateValue.scale.x +
        ", " +
        updateValue.scale.y +
        ")" +
        "skew(" +
        updateValue.skew.y +
        "deg," +
        updateValue.skew.x +
        "deg)";
      animation.timings = 1000 * value.time;
      animationStack.push(animation);
    }

    //play animation
    const moveUpanimation = this.animationBuilder.build(animationStack);
    const player = moveUpanimation.create(ele);
    player.play();
    player.onDone(() => {
      if (animationFinish) {
        animationFinish(ele);
      }
    });
  }

  //@move div at x and y
  static moveTo(
    ele: HTMLElement,
    position: Vec2,
    time: number,
    animationFinish: CallbackFunctionVariadic = undefined
  ) {
    const moveUpanimation = this.animationBuilder.build([
      animate(
        time * 1000,
        style({
          transform: "translate(" + position.x + "px, " + position.y + "px)"
        })
      )
    ]);
    const player = moveUpanimation.create(ele);
    player.play();
    player.onDone(() => {
      if (animationFinish) {
        animationFinish(ele);
      }
    });
  }
  //@rotate div
  static rotateTo(
    ele: HTMLElement,
    deg: number,
    time: number,
    animationFinish: CallbackFunctionVariadic = undefined
  ) {
    //rotate angle turn
    const rotateanimation = this.animationBuilder.build([
      animate(time * 1000, style({ transform: "rotate(" + deg + "deg)" }))
    ]);
    const player = rotateanimation.create(ele);
    player.play();
    player.onDone(() => {
      if (animationFinish) {
        animationFinish(ele);
      }
    });
  }
  //move and turn div
  static turnBy(
    ele: HTMLElement,
    total: number,
    time: number,
    animationFinish: CallbackFunctionVariadic = undefined
  ) {
    const rotateanimation = this.animationBuilder.build([
      animate(time * 1000, style({ transform: "rotate(" + total + "turn)" }))
    ]);
    const player = rotateanimation.create(ele);
    player.play();
    player.onDone(() => {
      if (animationFinish) {
        animationFinish(ele);
      }
    });
  }
  //rotate and move
  static rotateAndMove(
    ele: HTMLElement,
    position: Vec3,
    time: number,
    animationFinish: CallbackFunctionVariadic = undefined
  ) {
    const rotateAndMoveAnimation = this.animationBuilder.build([
      animate(
        time * 1000,
        style({
          transform:
            "translate(" +
            position.x +
            "px, " +
            position.y +
            "px)" +
            "rotate(" +
            position.deg +
            "deg)"
        })
      )
    ]);
    const player = rotateAndMoveAnimation.create(ele);
    player.play();
    player.onDone(() => {
      if (animationFinish) {
        animationFinish(ele);
      }
    });
  }
  //turn and move
  static turnAndMove(
    ele: HTMLElement,
    position: Vec3,
    time: number,
    animationFinish: CallbackFunctionVariadic = undefined
  ) {
    const rotateAndTurnAnimation = this.animationBuilder.build([
      animate(
        time * 1000,
        style({
          transform:
            "translate(" +
            position.x +
            "px, " +
            position.y +
            "px)" +
            "rotate(" +
            position.deg +
            "turn)"
        })
      )
    ]);
    const player = rotateAndTurnAnimation.create(ele);
    player.play();
    player.onDone(() => {
      if (animationFinish) {
        animationFinish(ele);
      }
    });
  }
  //rotate move and scale

  //rotate and scale

  //roate turn scale

  //chain animation
}

@Injectable()
export class LoaderService {
  constructor() {}
  public progressCallBack: CallbackFunctionVariadic;
  //load multiple images
  async loadImages(args: Array<string>) {
    let promiseArray = [];
    const totalImages = args.length;
    let loadedImages = 0;
    for (let i = 0; i < totalImages; i++) {
      promiseArray.push(
        new Promise((accept, reject) => {
          const image = new Image();
          image.src = args[i];
          image.onload = evt => {
            loadedImages++;
            const per = Number((loadedImages * 100) / totalImages);
            if (this.progressCallBack) {
              this.progressCallBack(per);
            }
            accept(evt);
          };
        })
      );
    }
    return new Promise((accept, recject) => {
      Promise.all(promiseArray)
        .then(r => {
          accept(true);
        })
        .catch(Error => {
          console.log(Error);
        });
    });
  }
}
