import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  constructor(public auth: AuthService, private router: RouterModule) {
  }

  isExpanded = false;

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  logout(): boolean {
    // logs out the user, then redirects him to Home View.
    if (this.auth.logout()) {
      //this.router.navigate([""]);
      console.log('logout clicked');
    }
    return false;
  }
}
