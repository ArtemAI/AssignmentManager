import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { faUser } from '@fortawesome/free-regular-svg-icons';
import { faLock } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'register',
  templateUrl: 'register.component.html'
})
export class RegisterComponent {

  registerForm: FormGroup;
  errorMessage: string;
  userIcon = faUser;
  lockIcon = faLock;

  constructor(private formBuilder: FormBuilder, private authService: AuthService, private router: Router) { }

  ngOnInit() {
    this.registerForm = this.formBuilder.group({
      'username': ['', Validators.required],
      'firstname': ['', Validators.required],
      'lastname': ['', Validators.required],
      'email': ['', Validators.email],
      'password': ['', Validators.required],
      'allowEmailNotifications': ['true'],
    });
  }

  register() {
    if (this.registerForm.valid) {
      this.authService.register(this.registerForm.value)
        .subscribe(
          () => {
            this.router.navigateByUrl('/');
          }, registerError => {
            this.errorMessage = registerError.error.message;
          }
        );
    }
  }
}
