import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
//import { AppComponent } from './app.component';
import { ProductComponent } from "./component";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";

@NgModule({
    declarations: [ProductComponent],
    imports: [BrowserModule, FormsModule, ReactiveFormsModule],
    providers: [],
    bootstrap: [ProductComponent]
})
export class AppModule { }