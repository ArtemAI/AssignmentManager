import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { faUser } from '@fortawesome/free-regular-svg-icons';
import { faLock } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'login',
  templateUrl: 'login.component.html'
})
export class LoginComponent {

  loginForm: FormGroup;
  errorMessage: string;
  userIcon = faUser;
  lockIcon = faLock;

  constructor(private formBuilder: FormBuilder, private authService: AuthService, private router: Router) { }

  ngOnInit() {
    this.loginForm = this.formBuilder.group({
      'username': ['', Validators.required],
      'password': ['', Validators.required],
    });
  }

  login() {
    this.authService.login(this.loginForm.value).subscribe(
      () => {
        this.router.navigateByUrl('/');
      }, loginError => {
        this.errorMessage = loginError.error;
      }
    );
  }

  logout() {
    this.authService.logout();
    this.router.navigateByUrl('/login');
  }

  redirectToRegisterPage() {
    this.router.navigateByUrl('/register')
  }
}
