import { HttpClient, HttpParams, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from 'src/environments/environment.development';
import { DisplayIngredientDTO } from '../../models/ingredients/display-ingredient';
import { PaginatedResult } from '../../models/pagination/pagination'
import { map, catchError, tap } from 'rxjs/operators';
import { FormArray, FormGroup } from '@angular/forms';

@Injectable({
  providedIn: 'root'
})
export class IngredientService {
  private refreshTableSubject = new BehaviorSubject<boolean>(false);

  constructor(private http: HttpClient) { }

  refreshTable$ = this.refreshTableSubject.asObservable();

  public triggerRefreshTable() {
    this.refreshTableSubject.next(true);
  }

  public getPagination(ingredientUrl: string, pageNumber?: number, pageSize?: number)
  : Observable<PaginatedResult<DisplayIngredientDTO[]>> 
  {
    // declaring variables
    let url = (`${environment.apiUrl}/${ingredientUrl}`)
    const paginatedResults: PaginatedResult<DisplayIngredientDTO[]> = new PaginatedResult<DisplayIngredientDTO[]>();
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


  public getIngredient(ingredientUrl: string, id: number) : Observable<DisplayIngredientDTO> {
    let url = (`${environment.apiUrl}/${ingredientUrl}/${id}`)
    let result = this.http.get<DisplayIngredientDTO>(url)
    console.log('res.pipe', result);
    return result
  }

  public getIngredients(ingredientUrl: string): Observable<DisplayIngredientDTO[]> {
    // sending request
    let url = (`${environment.apiUrl}/${ingredientUrl}`)
    console.log(url);
    let result = this.http.get<DisplayIngredientDTO[]>(url)
    return result
  }

  public create(form: FormGroup, ingredientUrl: string) {
    // sending request
    const formData = new FormData();
    const formValue = form.getRawValue()
    formData.append('objectFromPage', JSON.stringify(form.getRawValue()))


    let url = (`${environment.apiUrl}/${ingredientUrl}`)
    console.log(url);
    let result = this.http.post(url, formData, {withCredentials : true, responseType: 'text'})
    .subscribe((res: string) => {
      return 200
      }, 
      ((error: HttpErrorResponse) => {
        
        return error;
      })
    )
  }

  public save(form: FormGroup, ingredientUrl: string, method: string): Observable<any> {
    // sending request
    const formData = new FormData();
    const formValue = form.getRawValue()
    for (const field in formValue) {
      formData.append(field, formValue[field]);
    }

    let url = (`${environment.apiUrl}/${ingredientUrl}`)
    let result : Observable<any> = new Observable<any>()
    if (method == 'PUT')
      result = this.http.put(url, formData, {withCredentials : true, responseType: 'text'})
    else if (method == 'POST')
      result = this.http.post(url, formData, {withCredentials : true, responseType: 'text'})
    return result
  }
  public update(form: FormGroup, ingredientUrl: string): Observable<any> {
    // sending request
    const formData = new FormData();
    const formValue = form.getRawValue()
    formData.append('objectFromPage', JSON.stringify(form.getRawValue()))

    let url = (`${environment.apiUrl}/${ingredientUrl}`)
    return this.http.put(url, formData, {withCredentials : true, responseType: 'text'})
  }
  public delete(ingredientUrl: string, id: number): Observable<any> {
    console.log(`${environment.apiUrl}/${ingredientUrl}/${id}`)
      return this.http.delete(`${environment.apiUrl}/${ingredientUrl}/${id}`, {withCredentials : true, responseType: 'text'})
  }
}
