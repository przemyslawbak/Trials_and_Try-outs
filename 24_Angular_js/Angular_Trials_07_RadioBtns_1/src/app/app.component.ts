import { Component } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  public multiplayer: boolean = false;
  private speed: number = 30;

  form = new FormGroup({
    gameMode: new FormControl('single', Validators.required),
    gameSpeed: new FormControl('slow', Validators.required),
    gameOpen: new FormControl('open'),
    gameDifficulty: new FormControl('easy'),
  });

  get f() {
    return this.form.controls;
  }

  public submit() {
    console.log(this.form.value);
  }

  public changeMode(e: any): void {
    if (e.target.value == 'multi') {
      this.multiplayer = true;
    } else {
      this.multiplayer = false;
    }
  }

  public changeSpeed(e: any): void {
    console.log(e.target.value); //todo:
  }

  public changeDifficulty(e: any): void {
    console.log(e.target.value); //todo:
  }

  public changeJoining(e: any): void {
    console.log(e.target.value); //todo:
  }
}
