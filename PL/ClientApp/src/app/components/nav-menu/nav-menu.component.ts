import { Component } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { faSignOutAlt } from '@fortawesome/free-solid-svg-icons';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  userName: any;
  isUserLoggedIn: boolean = this.authService.isLoggedIn();
  logOutIcon = faSignOutAlt;

  constructor(private authService: AuthService, private router: Router) { }

  ngOnInit(): void {
    this.authService.getLoggedInName.subscribe(name => {
      this.userName = name;
      this.isUserLoggedIn = true;
    });
  }

  logUserOut() {
    this.authService.logout();
  }
}
