import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import { FormsModule } from "@angular/forms";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { AppComponent } from "./app.component";
import { AnimatedSpriteComponent } from "./animated-sprite/animated-sprite.component";
import { LoaderService } from "./services/loader.service";

@NgModule({
  imports: [BrowserModule, BrowserAnimationsModule, FormsModule],
  declarations: [AppComponent, AnimatedSpriteComponent],
  bootstrap: [AppComponent],
  providers: [LoaderService]
})
export class AppModule {}
