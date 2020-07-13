import { AuthService } from './services/auth.service';
import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {
  title = 'app';
  isUserLoggedIn: boolean = this.authService.isLoggedIn();

  constructor(private authService: AuthService) { }
}
