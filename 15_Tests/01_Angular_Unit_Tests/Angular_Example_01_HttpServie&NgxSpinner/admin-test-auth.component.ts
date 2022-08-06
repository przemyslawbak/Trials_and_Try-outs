import { Component, OnInit } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { environment } from '@environments/environment';

import { AuthService } from '@services/auth.service';
import { HttpService } from '@services/http.service';

@Component({
  templateUrl: './admin-test-auth.component.html',
  styleUrls: ['./admin-test-auth.component.css'],
})
export class AdminComponent implements OnInit {
  constructor(
    private http: HttpService,
    public auth: AuthService,
    private spinner: NgxSpinnerService
  ) {}

  public ngOnInit(): void {
    this.executeCall();
  }

  private executeCall(): void {
    this.spinner.show();
    const url = environment.apiUrl + 'api/user/admin';
    this.http.getData(url).subscribe((val) => {
      console.log('GET call successful value returned in body', val);
      this.spinner.hide();
    });
  }
}
