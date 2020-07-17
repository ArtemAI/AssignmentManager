import { Component } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { faSignOutAlt } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  userName: any;
  isUserLoggedIn: boolean = this.authenticationService.isLoggedIn();
  logOutIcon = faSignOutAlt;

  constructor(private authenticationService: AuthService) { }

  ngOnInit(): void {
    this.authenticationService.getLoggedInName.subscribe(name => {
      this.userName = name;
      this.isUserLoggedIn = true;
    });
  }

  logUserOut() {
    this.authenticationService.logout();
  }
}
