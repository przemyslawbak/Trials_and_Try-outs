import { Component } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  form = new FormGroup({
    gameMode: new FormControl('single', Validators.required),
    gameSpeed: new FormControl('slow', Validators.required),
  });

  get f() {
    return this.form.controls;
  }

  ngAfterViewInit() {
    //
  }

  submit() {
    console.log(this.form.value);
  }

  changeMode(e: any) {
    console.log(e.target.value);
  }

  changeSpeed(e: any) {
    console.log(e.target.value);
  }
}
