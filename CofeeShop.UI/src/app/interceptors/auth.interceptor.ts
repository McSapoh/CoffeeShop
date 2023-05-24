import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse,
  HttpClient
} from '@angular/common/http';
import { catchError, Observable, switchMap, throwError } from 'rxjs';
import { environment } from 'src/environments/environment.development';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  refresh = false

  constructor(private http: HttpClient) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    const editerRequest = request.clone({
      withCredentials: true,
      setHeaders: {
        Authorization: `Bearer ${localStorage.getItem('accessToken')}`
      }
    })
    console.log(editerRequest);

    return next.handle(editerRequest).pipe(catchError((error: HttpErrorResponse) => {
      if (error.status === 401 && !this.refresh) {
        if (!this.refresh) {
          this.refresh = true

          let url = `${environment.apiUrl}/Auth/refresh-token`
          return this.http.post(url, {},
          {withCredentials : true, responseType: 'text'}).pipe(
            switchMap((res: any) => {
              localStorage.setItem('accessToken', res);


              return next.handle(editerRequest.clone({
                withCredentials: true,
                setHeaders: {
                  Authorization: `Bearer ${res}`
                }
              }))
            })
          )
        }
        this.refresh = false
        window.location.href = 'http://localhost:4200/Login'
      }
      
      return throwError(() => error)
    }));
  }
}
