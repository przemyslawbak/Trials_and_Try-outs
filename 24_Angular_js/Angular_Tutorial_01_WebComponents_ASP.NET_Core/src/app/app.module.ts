import { BrowserModule } from '@angular/platform-browser';
import { NgModule, Injector } from '@angular/core';
import { createCustomElement } from '@angular/elements';
import { APP_BASE_HREF } from '@angular/common';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CurrentTimeComponent } from './current-time/current-time.component';

@NgModule({
  declarations: [
    AppComponent,
    CurrentTimeComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule
  ],
  exports: [
    CurrentTimeComponent
  ],
  providers: [{ provide: APP_BASE_HREF, useValue: '/' }],
  entryComponents: [CurrentTimeComponent]
})
export class AppModule {
  constructor(private injector: Injector) {
  }

  ngDoBootstrap() {
    customElements.define('current-time', createCustomElement(CurrentTimeComponent,
      { injector: this.injector }));
  }
}
