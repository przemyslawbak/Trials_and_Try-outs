import { Component } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  multiplayer: boolean = false;
  gamespeed: number = 30;

  form = new FormGroup({
    gameMode: new FormControl('single', Validators.required),
    gameSpeed: new FormControl('slow', Validators.required),
    gameOpen: new FormControl('open'),
  });

  get f() {
    return this.form.controls;
  }

  ngAfterViewInit() {
    //no need?
  }

  submit() {
    console.log(this.form.value);
  }

  changeMode(e: any) {
    if (e.target.value == 'multi') {
      this.multiplayer = true;
    } else {
      this.multiplayer = false;
    }
  }

  changeSpeed(e: any) {
    console.log(e.target.value);
  }

  isMultiDisabled(): boolean {
    return true;
  }
}
