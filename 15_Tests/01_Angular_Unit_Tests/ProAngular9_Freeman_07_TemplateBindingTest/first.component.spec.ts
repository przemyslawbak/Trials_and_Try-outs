import { TestBed, ComponentFixture } from '@angular/core/testing';
import { FirstComponent } from '../ondemand/first.component';
import { Product } from '../model/product.model';
import { Model } from '../model/repository.model';
import { DebugElement } from '@angular/core';
import { By } from '@angular/platform-browser';

describe('FirstComponent', () => {
  let fixture: ComponentFixture<FirstComponent>;
  let component: FirstComponent;
  let debugElement: DebugElement;
  let bindingElement: HTMLSpanElement;
  let mockRepository = {
    getProducts: function () {
      return [
        new Product(1, 'test1', 'Soccer', 100),
        new Product(2, 'test2', 'Chess', 100),
        new Product(3, 'test3', 'Soccer', 100),
      ];
    },
  };
  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [FirstComponent],
      providers: [{ provide: Model, useValue: mockRepository }],
    });
    fixture = TestBed.createComponent(FirstComponent);
    component = fixture.componentInstance;
    debugElement = fixture.debugElement;
    bindingElement = debugElement.query(By.css('span')).nativeElement;
  });
  it('filters categories', () => {
    component.category = 'Chess';
    fixture.detectChanges();
    expect(component.getProducts().length).toBe(1);
    expect(bindingElement.textContent).toContain('1');
    component.category = 'Soccer';
    fixture.detectChanges();
    expect(component.getProducts().length).toBe(2);
    expect(bindingElement.textContent).toContain('2');
    component.category = 'Running';
    fixture.detectChanges();
    expect(component.getProducts().length).toBe(0);
    expect(bindingElement.textContent).toContain('0');
  });
});
