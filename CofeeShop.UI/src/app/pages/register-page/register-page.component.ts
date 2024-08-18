import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { EditUserDTO } from 'src/app/models/user/edit';
import { AuthService } from 'src/app/services/auth/auth.service';

@Component({
  selector: 'app-register-page',
  templateUrl: './register-page.component.html'
})
export class RegisterPageComponent {
  form!: FormGroup
  constructor(
    private formBuilder: FormBuilder,
    private service: AuthService,
  ) {}

  ngOnInit() {
    let model = new EditUserDTO();
    this.form = this.formBuilder.group({
      Name: [model.Name, Validators.required],
      Email: [model.Email, [Validators.required, Validators.email]],
      Password: [model.Password, Validators.required],
    });

    // Subscribe to value changes to remove errors when inputs are valid
    this.form.get('Name')?.valueChanges.subscribe(() => {
      if (this.form.get('Name')?.valid && this.form.get('Name')?.touched) {
        this.form.get('Name')?.setErrors(null);
      }
    });

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

  register() {
    this.service.register(this.form)
  }
}
