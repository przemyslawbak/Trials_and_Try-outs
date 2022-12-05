import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { AthleteService } from './athlete.service';
import { AthleteListComponent } from './athlete-list.component';
import { AthleteComponent } from './athlete.component';

@NgModule({
  declarations: [
    AppComponent,
    AthleteListComponent,
    AthleteComponent,
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule
  ],
  providers: [AthleteService],
  bootstrap: [AppComponent]
})
export class AppModule { }
