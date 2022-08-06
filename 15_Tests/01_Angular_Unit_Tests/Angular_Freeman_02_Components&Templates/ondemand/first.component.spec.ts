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
  let spanElement: HTMLSpanElement;
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
      spanElement = debugElement.query(By.css("span")).nativeElement;
    });
  }));
  it("filters categories", () => {
    component.category = "Chess";
    fixture.detectChanges();
    expect(component.getProducts().length).toBe(1);
    expect(spanElement.textContent).toContain("1");
  });
});
