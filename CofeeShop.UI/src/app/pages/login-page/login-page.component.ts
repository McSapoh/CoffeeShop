import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LoginUserDTO } from 'src/app/models/user/login';
import { AuthService } from 'src/app/services/auth/auth.service';

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
    let model = new LoginUserDTO();
    this.form = this.formBuilder.group({
      Email: [model.Email, [Validators.required, Validators.email]],
      Password: [model.Password, Validators.required],
    });

    // Subscribe to value changes to remove errors when inputs are valid
    this.form.get('Email')?.valueChanges.subscribe(() => {
      if (this.form.get('Email')?.valid && this.form.get('Email')?.touched) {
        this.form.get('Email')?.setErrors(null);
      }
    });

    this.form.get('Password')?.valueChanges.subscribe(() => {
      if (this.form.get('Password')?.valid && this.form.get('Password')?.touched) {
        this.form.get('Password')?.setErrors(null);
      }
    });
  }
  


  login() {
    if (this.form.valid) {
      this.service.login(this.form)
    } else {
      this.form.markAllAsTouched();
    }
  }
  
}
