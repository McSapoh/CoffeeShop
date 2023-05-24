import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { throwError } from 'rxjs';
import { AuthInterceptor } from 'src/app/interceptors/auth.interceptor';
import { LoginUserDTO } from 'src/app/models/user/login';
import { AuthService } from 'src/app/services/auth/auth.service';
import { environment } from 'src/environments/environment.development';

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html'
})
export class LoginPageComponent {
  form!: FormGroup
  constructor(
    private formBuilder: FormBuilder,
    private service: AuthService
  ) {}

  ngOnInit() {
    let model = new LoginUserDTO()
    this.form = this.formBuilder.group({
      Email: [model.Email, [Validators.required, Validators.email]],
      Password: [model.Password, Validators.required]
    })
  }

  login() {
    this.service.login(this.form)
  }
}
