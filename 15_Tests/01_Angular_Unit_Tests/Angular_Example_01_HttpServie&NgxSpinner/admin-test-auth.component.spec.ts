import { TestBed, ComponentFixture } from '@angular/core/testing';
import {
  HttpClientTestingModule,
  HttpTestingController,
} from '@angular/common/http/testing';
import { NgxSpinnerModule } from 'ngx-spinner';
import { AdminComponent } from './admin-test-auth.component';
import { AuthService } from '@services/auth.service';
import { HttpService } from '@services/http.service';
import { SecurityService } from '@services/security.service';
import { RouterTestingModule } from '@angular/router/testing';

describe('AdminComponent', () => {
  let httpMock: HttpTestingController;
  let httpService: HttpService;

  let fixture: ComponentFixture<AdminComponent>;
  let component: AdminComponent;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, NgxSpinnerModule, RouterTestingModule],
      providers: [AuthService, SecurityService, HttpService],
      declarations: [AdminComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(AdminComponent);
    component = fixture.componentInstance;

    httpService = TestBed.inject(HttpService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify(); //Verifies that no requests are outstanding.
  });

  //only a trial test
  it('should method be called', () => {
    spyOn(component, 'ngOnInit');

    component.ngOnInit();

    expect(component.ngOnInit).toHaveBeenCalled();
  });

  //only a trial test
  it('should be http get called', () => {
    spyOn(component, 'ngOnInit');

    component.ngOnInit();

    expect(component.ngOnInit).toHaveBeenCalled();
  });

  //only a trial test
  it('getData() should http GET names', () => {
    const names = [{ name: 'a' }, { name: 'b' }];

    httpService.getData('/app/data').subscribe((res) => {
      expect(res).toEqual(names);
    });

    const req = httpMock.expectOne('/app/data');
    expect(req.request.method).toEqual('GET');
    req.flush(names);

    httpMock.verify();
  });
});
