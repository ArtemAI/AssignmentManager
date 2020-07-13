import { Component } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;
  userName: any;
  isUserLoggedIn: boolean = this.authenticationService.isLoggedIn();

  constructor(private authenticationService: AuthService) { }

  ngOnInit(): void {
    this.authenticationService.getLoggedInName.subscribe(name => {
      this.userName = name;
      this.isUserLoggedIn = true;
    });
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
