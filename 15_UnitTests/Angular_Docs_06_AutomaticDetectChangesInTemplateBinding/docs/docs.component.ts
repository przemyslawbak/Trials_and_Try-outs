import { Component } from '@angular/core';
@Component({
  selector: 'docs',
  template: '<h1>{{title}}</h1>',
  styles: ['h1 { color: green; font-size: 350%}'],
})
export class DocsComponent {
  title = 'Test Tour of Heroes';
}
