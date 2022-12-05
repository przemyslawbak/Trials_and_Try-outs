import { TestBed, ComponentFixture, async } from "@angular/core/testing";
import { FirstComponent } from "../ondemand/first.component";
import { Product } from "..//model/product.model";
import { Model } from "../model/repository.model";
import { DebugElement } from "@angular/core";
import { By } from "@angular/platform-browser";

describe("FirstComponent", () => {
  let fixture: ComponentFixture<FirstComponent>;
  let component: FirstComponent;
  let debugElement: DebugElement;
  let divElement: HTMLDivElement;

  let mockRepository = {
    getProducts: function () {
      return [
        new Product(1, "test1", "Soccer", 100),
        new Product(2, "test2", "Chess", 100),
        new Product(3, "test3", "Soccer", 100),
      ];
    },
  };
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [FirstComponent],
      providers: [{ provide: Model, useValue: mockRepository }],
    });
    TestBed.compileComponents().then(() => {
      fixture = TestBed.createComponent(FirstComponent);
      component = fixture.componentInstance;
      debugElement = fixture.debugElement;
      divElement = debugElement.children[0].nativeElement;
    });
  }));
  it("handles mouse events", () => {
    expect(component.highlighted).toBeFalsy();
    expect(divElement.classList.contains("bg-success")).toBeFalsy();
    debugElement.triggerEventHandler("mouseenter", new Event("mouseenter"));
    fixture.detectChanges();
    expect(component.highlighted).toBeTruthy();
    expect(divElement.classList.contains("bg-success")).toBeTruthy();
    debugElement.triggerEventHandler("mouseleave", new Event("mouseleave"));
    fixture.detectChanges();
    expect(component.highlighted).toBeFalsy();
    expect(divElement.classList.contains("bg-success")).toBeFalsy();
  });
});
