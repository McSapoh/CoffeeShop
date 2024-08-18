import { Component } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { DisplayProductDTO } from 'src/app/models/products/display-product';
import { Inject } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ProductService } from 'src/app/services/product/product.service';
import { Size } from 'src/app/models/size';
import { HttpErrorResponse } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';
import { ProductTableComponent } from '../../product-table/product-table.component';
import { environment } from 'src/environments/environment.development';

@Component({
  selector: 'app-product-popup',
  templateUrl: './product-popup.component.html'
})
export class ProductPopupComponent {
  form!: FormGroup;
  sizesFormGroup!: FormGroup;
  sizes!: FormArray;
  product: DisplayProductDTO;
  productType = '';
  id: string | undefined = '' ;
  table: ProductTableComponent
  currentPage?: number
  pageSize?: number
  imageUrl: string | ArrayBuffer | null = null;
  defaultImageUrl = environment.apiUrl + '/images'

  constructor(
    private formBuilder: FormBuilder, 
    private service: ProductService,
    private dialogRef: MatDialogRef<ProductPopupComponent>,
    private toastr: ToastrService, 
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    console.log('popup', data);
    this.product = data.product;
    this.productType = data.productType;
    this.table = data.table;
    this.currentPage = data.currentPage;
    this.pageSize = data.pageSize;

    if (!this.product) {
      this.product = new DisplayProductDTO();
    } else {
      this.id = this.product.id?.toString();
    }

    // this.sizes = this.form.get('Sizes') as FormArray;

    this.sizes = this.formBuilder.array([])
    if (this.product.sizes.length == 0) {
      this.product.sizes.push(new Size())
    }
    this.product.sizes.forEach(size => {
      this.addSizeFormGroup(size)
    });

    this.form = this.formBuilder.group({
      Name: [this.product.name, [Validators.required]],
      Description: [this.product.description, Validators.required],
      IsActive: [this.product.isActive],
      Sizes: this.sizes,
      
    });
    this.imageUrl = this.product.imagePath ? (this.defaultImageUrl + '/' + this.product.imagePath) : ('assets/img/' + this.productType + '.png');

    this.sizes = this.form.get('Sizes') as FormArray;
  }
  onFileSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.onload = (e: any) => {
        this.imageUrl = e.target.result;
      };
      reader.readAsDataURL(file);
    }
  }
  createSizeFormGroup(size: Size = new Size()): FormGroup {
    return this.formBuilder.group({
      Id: [size.id],
      Name: [size.name, Validators.required], //Validators.required
      Description: [size.description, Validators.required],
      Price: [size.price],
    })
  }

  addSizeFormGroup(size: Size = new Size()) {
    console.log(size);
    this.sizes.push(this.createSizeFormGroup(size));
  }

  removeSizeFormGroup(index: number) {
    this.sizes.removeAt(index);
  }

  save(): void {
    if (this.form.invalid)
      this.markFormGroupDirty(this.form);

    // Check if the form is valid
    if (this.form.valid) {
      const product: DisplayProductDTO = this.form.value;
      console.log(product);
      let method: string = 'POST'
      let url: string = this.productType

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
              } else if (prop.startsWith('Sizes[')) {
                const match = prop.match(/\d+/);
                if (match) {
                  const index = parseInt(match[0]);
                  const sizeControl = this.sizes.at(index)?.get(prop.split('.')[1]);
                  if (sizeControl) {
                    sizeControl.setErrors({ serverError: validationErrors[prop] });
                  }
                }
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