import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpErrorResponse } from '@angular/common/http';
import { environment } from 'src/environments/environment.development';
import { FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, catchError, map, of, throwError } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { JwtHelperService } from '@auth0/angular-jwt';


@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private jwtHelper: JwtHelperService
  constructor(private http: HttpClient,
    private router: Router,        
    private route: ActivatedRoute,
    private toastr: ToastrService    
  ) { 
    this.jwtHelper = new JwtHelperService()
  }

  register(form: FormGroup) {    
    let url = (`${environment.apiUrl}/Auth/Register`)
    console.log(form.getRawValue())
    this.http.post(url, form.getRawValue(), {withCredentials : true, responseType: 'text'})
      .subscribe((res: string) => {
        console.log(form.getRawValue())    
        this.toastr.success(res);
      }, 
      ((error: HttpErrorResponse) => {
        // Here you can handle the error and retrieve the HTTP status code
        const statusCode = error.status;

        // NotFound result
        if (statusCode == 404) {
          this.toastr.error('Not found user with current', 'Not Found');
        }
        // Validation error
        if (statusCode == 400) {
          let validationErrors = JSON.parse(error.error)
          Object.keys(validationErrors).forEach(prop => {
            const formControl = form.get(prop);
            if (formControl) {
              formControl.setErrors({
                serverError: validationErrors[prop]
              });
            }
          });
        }
        // Do something with the status code
        return throwError(error);
      })
    )
  }
  login(form: FormGroup) {    
    let url = (`${environment.apiUrl}/Auth/Login`)
    this.http.post(url, form.getRawValue(), {withCredentials : true, responseType: 'text'})
      .subscribe((res: string) => {
        localStorage.setItem('accessToken', res);
        let tokenD = this.jwtHelper.decodeToken(res) 
        localStorage.setItem('username', tokenD.name);
        localStorage.setItem('profileImageUrl', `${environment.apiUrl}/images/${tokenD.profileImagePath}`);
      
        const returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
        this.router.navigateByUrl(returnUrl);
      }, 
      ((error: HttpErrorResponse) => {
        // Here you can handle the error and retrieve the HTTP status code
        const statusCode = error.status;

        // NotFound result
        if (statusCode == 404) {
          this.toastr.error('Not found user with current', 'Not Found');
        }
        // Validation error
        if (statusCode == 400) {
          let validationErrors = JSON.parse(error.error)
          Object.keys(validationErrors).forEach(prop => {
            const formControl = form.get(prop);
            if (formControl) {
              formControl.setErrors({
                serverError: validationErrors[prop]
              });
            }
          });
        }
        // Do something with the status code
        return throwError(error);
      })
    )
  }
  refreshToken(): Observable<boolean> {
    let url = `${environment.apiUrl}/Auth/refresh-token`;
  
    return this.http.post(url, '', { withCredentials: true, responseType: 'text' }).pipe(
      map((res: string) => {
        localStorage.setItem('accessToken', res);
        let tokenD = this.jwtHelper.decodeToken(res);
        localStorage.setItem('username', tokenD.name);
        localStorage.setItem('profileImageUrl', `${environment.apiUrl}/images/${tokenD.profileImagePath}`);
        return true;
      }),
      catchError((error: HttpErrorResponse) => {
        return of(false);
      })
    );
  }
  isLoggedIn(): boolean {
    var token = localStorage.getItem('accessToken')
    console.log(token);
    return !this.jwtHelper.isTokenExpired(token)
  }
  logout() {
    localStorage.removeItem('accessToken')
    this.router.navigate(['/Login'])
  }
  externalLogin(provider: string = 'google') {
    let url = (`${environment.apiUrl}/ExternalAuth/signin-by-${provider}`)
    this.http.get(url, {withCredentials : true, responseType: 'text'})
      .subscribe((res: string) => {
        localStorage.setItem('accessToken', res);
        let tokenD = this.jwtHelper.decodeToken(res) 
        localStorage.setItem('username', tokenD.name);
        localStorage.setItem('profileImageUrl', `${environment.apiUrl}/images/${tokenD.profileImagePath}`);
      
        const returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
        this.router.navigateByUrl(returnUrl);
      }, 
      ((error: HttpErrorResponse) => {
        // Here you can handle the error and retrieve the HTTP status code
        const statusCode = error.status;

        // NotFound result
        if (statusCode == 404) {
          this.toastr.error('Not found user with current', 'Not Found');
        }
        // Validation error
        if (statusCode == 400) {
          
        }
        // Do something with the status code
        return throwError(error);
      })
    )
  }
}
