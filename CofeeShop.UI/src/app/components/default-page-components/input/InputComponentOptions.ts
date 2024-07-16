import { AbstractControl } from "@angular/forms"

export class InputComponentOptions {
    public control: AbstractControl<any, any> | null = null
    public inputType: string = ''
    public formControlName: string = ''
    public placeholder: string = ''
    public isRequired: boolean= false
    public isServerError: boolean= false
    public isEmail: boolean = false
  
  
    // constructor(public data: any) {
    //   this.control = data.control
    //   this.inputType = data.inputType
    //   this.formControlName = data.formControlName
    //   this.placeholder = data.placeholder
    //   this.isRequired = data.isRequired
    //   this.isServerError = data.isServerError
    //   this.isRequired = data.isRequired
    //   this.isEmail = data.isEmail
    // }
  }