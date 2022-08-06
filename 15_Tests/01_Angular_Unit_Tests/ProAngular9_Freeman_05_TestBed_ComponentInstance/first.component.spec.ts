import { TestBed, ComponentFixture } from '@angular/core/testing';
import { FirstComponent } from '../ondemand/first.component';
describe('FirstComponent', () => {
  let fixture: ComponentFixture<FirstComponent>;
  let component: FirstComponent;
  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [FirstComponent],
    });
    fixture = TestBed.createComponent(FirstComponent);
    component = fixture.componentInstance;
  });
  it('is defined', () => {
    expect(component).toBeDefined();
  });
});
