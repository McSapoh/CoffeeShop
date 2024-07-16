import { Component, Inject } from '@angular/core';
import { FormGroup, FormBuilder, Validators, AbstractControl } from '@angular/forms';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { LoginUserDTO } from 'src/app/models/user/login';
import { AuthService } from 'src/app/services/auth/auth.service';
import { InputComponentOptions } from './InputComponentOptions';

@Component({
  selector: 'app-input',
  template: `
  <div class="input-group mb-3">
    <input type= {{options.inputType}} class="form-control" 
      placeholder = {{options.placeholder}} formControlName = {{options.formControlName}} 
      required = {{options.isRequired}}
    >
    <div class="input-group-append">
      <div class="input-group-text">
        <span class="fas fa-envelope"></span>
      </div>
    </div>
  </div>


  <mat-error *ngIf="options.isRequired && options.control?.hasError('required')">
  {{options.placeholder}} is required.
  </mat-error>
  <mat-error *ngIf="options.isEmail && options.control?.hasError('email')">
  {{options.placeholder}} is invalid.
  </mat-error>
  <mat-error *ngIf="options.isServerError && options.control?.hasError('serverError')">
    {{ options.control?.getError('serverError')[0] }}
  </mat-error>`,
})
export class InputComponent {
  options: InputComponentOptions

  constructor(
    private formBuilder: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public data: any

  ) {
    this.options = data.options
  }
}
