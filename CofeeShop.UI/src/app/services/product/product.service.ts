import { HttpClient, HttpParams, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment.development';
import { DisplayProductDTO } from '../../models/products/display-product';
import { PaginatedResult } from '../../models/pagination/pagination'
import { map, catchError, tap } from 'rxjs/operators';
import { FormArray, FormGroup } from '@angular/forms';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  constructor(private http: HttpClient) { }
  
  public getPagination(productUrl: string, pageNumber?: number, pageSize?: number)
  : Observable<PaginatedResult<DisplayProductDTO[]>> 
  {
    // declaring variables
    let url = (`${environment.apiUrl}/${productUrl}`)
    const paginatedResults: PaginatedResult<DisplayProductDTO[]> = new PaginatedResult<DisplayProductDTO[]>();
    let params = new HttpParams();

    // setting parameters
    if (pageNumber != null && pageSize != null) {
      params = params.append('pageNumber', pageNumber);
      params = params.append('pageSize', pageSize);
    }
    // sending request
    return this.http.get<any>(
      url,
      { responseType: "json", observe: 'response', params, withCredentials: true })
      .pipe(map(res => {
        // getting body of request
        paginatedResults.result = res.body;

        // getting pagination headers
        let xpagination = res.headers.get('X-Pagination')
        if (xpagination != null) {
          paginatedResults.pagination = JSON.parse(xpagination)
        }
        return paginatedResults;
      }),);
  }


  public getProduct(productUrl: string, id: number) : Observable<DisplayProductDTO> {
    let url = (`${environment.apiUrl}/${productUrl}/${id}`)
    let result = this.http.get<DisplayProductDTO>(url)
    console.log('res.pipe', result);
    return result
  }

  public getProducts(productUrl: string): Observable<DisplayProductDTO[]> {
    // sending request
    let url = (`${environment.apiUrl}/${productUrl}`)
    console.log(url);
    let result = this.http.get<DisplayProductDTO[]>(url)
    return result
  }

  public create(form: FormGroup, productUrl: string) {
    // sending request
    let url = (`${environment.apiUrl}/${productUrl}`)
    console.log(url);
    let result = this.http.post(url, form.getRawValue(), {withCredentials : true, responseType: 'text'})
    .subscribe((res: string) => {
      return 200
      }, 
      ((error: HttpErrorResponse) => {
        
        return error;
      })
    )
  }

  public update(form: FormGroup, productUrl: string): Observable<any> {
    // sending request
    const formData = new FormData();
    const formValue = form.getRawValue()
    // formData.append('objectFromPage', JSON.stringify(form.getRawValue()))
    for (const field in formValue) {
      if (field !== 'Sizes') {
        formData.append(field, formValue[field]);
      }
    }

    // Add photo file from input to FormData
    const photoInput = document.querySelector('input[name="photo"]') as HTMLInputElement;
    if (photoInput?.files?.length) {
      const photoFile = photoInput.files[0];
      formData.append('photo', photoFile);
    }


    const sizesArray = form.get('Sizes') as FormArray;
    for (let i = 0; i < sizesArray.length; i++) {
      const sizeGroup = sizesArray.controls[i] as FormGroup;
      for (const field in sizeGroup.value) {
        formData.append(`Sizes[${i}].${field}`, sizeGroup.value[field]);
      }
    }


    let url = (`${environment.apiUrl}/${productUrl}`)
    return this.http.put(url, formData, {withCredentials : true, responseType: 'text'})
  }
}
