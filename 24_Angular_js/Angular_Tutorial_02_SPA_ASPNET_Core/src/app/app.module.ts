import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { APP_BASE_HREF } from '@angular/common'; //for ASP.NET Core

import { CurrentTimeComponent } from 'C:/Users/asus/Desktop/Integrate/Integrate/AngularApp/src/app/current-time/current-time.component';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { Page1Component } from './page1/page1.component';
import { Page2Component } from './page2/page2.component';

@NgModule({
  declarations: [
    AppComponent,
    Page1Component,
    Page2Component,
    CurrentTimeComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule
  ],

  providers: [{ provide: APP_BASE_HREF, useValue: '/' }], //for ASP.NET Core
  bootstrap: [AppComponent]
})
export class AppModule { }
