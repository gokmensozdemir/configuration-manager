import {
  HttpClient,
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, throwError } from 'rxjs';
import { catchError} from 'rxjs/operators';

/** Pass untouched request through to the next request handler. */
@Injectable({ providedIn: 'root' })
export class InterceptorService implements HttpInterceptor {
  constructor(
    private router: Router,
    private http: HttpClient
  ) {}
  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    req = req.clone();

    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse): Observable<any> => {

        if (error && error.status == 400) {
          let errMessages: string[] = [];
          let errors = error.error.errors;

          for(let err in errors){
            errMessages = errMessages.concat(errors[err]);
          }

          alert(errMessages.join('\n'));
        }

        return throwError(error);
      })
    );
  }
}
