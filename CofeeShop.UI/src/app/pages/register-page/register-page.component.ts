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
    let model = new EditUserDTO()
    this.form = this.formBuilder.group({
      Name: [model.Name, Validators.required],
      Adress: [model.Adress],
      Email: [model.Email, [Validators.required, Validators.email]],
      Password: [model.Password, Validators.required]
    })
  }

  register() {
    this.service.register(this.form)
  }
}
