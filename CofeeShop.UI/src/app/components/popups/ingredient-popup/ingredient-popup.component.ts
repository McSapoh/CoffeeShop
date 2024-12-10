import { Component } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Inject } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ProductService } from 'src/app/services/product/product.service';
import { Size } from 'src/app/models/size';
import { HttpErrorResponse } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';
import { environment } from 'src/environments/environment.development';
import { IngredientTableComponent } from '../../ingredient-table/ingredient-table.component';
import { DisplayIngredientDTO } from 'src/app/models/ingredients/display-ingredient';
import { IngredientService } from 'src/app/services/ingredient/ingredient.service';
@Component({
  selector: 'app-ingredient-popup',
  templateUrl: './ingredient-popup.component.html',
})
export class IngredientPopupComponent {
  form!: FormGroup;
  product: DisplayIngredientDTO;
  ingredientType = '';
  id: string | undefined = '' ;
  table: IngredientTableComponent
  currentPage?: number
  pageSize?: number

  constructor(
    private formBuilder: FormBuilder, 
    private service: IngredientService,
    private dialogRef: MatDialogRef<IngredientPopupComponent>,
    private toastr: ToastrService, 
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    console.log('popup', data);
    this.product = data.product;
    this.ingredientType = data.ingredientType;
    this.table = data.table;
    this.currentPage = data.currentPage;
    this.pageSize = data.pageSize;

    if (!this.product) {
      this.product = new DisplayIngredientDTO();
    } else {
      this.id = this.product.id?.toString();
    }

    this.form = this.formBuilder.group({
      Name: [this.product.name, [Validators.required]],
      Price: [this.product.price, Validators.required],
      IsActive: [this.product.isActive],      
    });
  }

  save(): void {
    if (this.form.invalid)
      this.markFormGroupDirty(this.form);

    // Check if the form is valid
    if (this.form.valid) {
      const product: DisplayIngredientDTO = this.form.value;
      console.log(product);
      let method: string = 'POST'
      let url: string = this.ingredientType

      if (this.id) {
        method = 'PUT'
        url += `/${this.id}`
      } 
      this.service.save(this.form, url, method)
      .subscribe((res: string) => {
          this.service.triggerRefreshTable();
          this.dialogRef.close()
          this.toastr.success('Successfully saved', res);
        }, ((error: HttpErrorResponse) => {
          if (error.status == 400) {
            console.log(error.error);
            let validationErrors = JSON.parse(error.error);
            Object.keys(validationErrors).forEach(prop => {
              if (this.form.get(prop)) {
                this.form.get(prop)?.setErrors({ serverError: validationErrors[prop] });
              } 
            });
            


          } else if (error.status == 404) {
            this.toastr.error('Not found product with current id', 'Not Found');
          } else if (error.status == 500) {
            this.toastr.error('Unknown error occurred while updating. \n Try again later please', 'Error');
          } else {
            this.toastr.error(error.message)
          }
          console.log(error);
        })
      )
    }
  }
  private markFormGroupDirty(formGroup: FormGroup) {
    Object.values(formGroup.controls).forEach(control => {
      if (control instanceof FormArray) {
        Object.values(control.controls).forEach(arraygroup => {
          if (arraygroup instanceof FormGroup) {
            Object.values(arraygroup.controls).forEach(gcontrol => {
              gcontrol.markAsDirty();
            });
          }
          
        });
      }
      control.markAsDirty();
    });
  }
}
