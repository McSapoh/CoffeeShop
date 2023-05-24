import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpErrorResponse } from '@angular/common/http';
import { environment } from 'src/environments/environment.development';
import { FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { throwError } from 'rxjs';
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

  login(form: FormGroup) {    
    let url = (`${environment.apiUrl}/Auth/Login`)
    this.http.post(url, form.getRawValue(), {withCredentials : true, responseType: 'text'})
      .subscribe((res: string) => {
        localStorage.setItem('accessToken', res);
        console.log('token',res);
        // this.router.navigate(['/'])

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

  isLoggedIn(): boolean {
    var token = localStorage.getItem('accessToken')
    console.log(token);
    return !this.jwtHelper.isTokenExpired(token)
  }

  logout() {
    localStorage.removeItem('accessToken')
    this.router.navigate([''])
  }
}
