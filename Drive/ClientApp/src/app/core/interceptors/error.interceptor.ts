import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs';
import { Router } from "@angular/router";

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private router: Router) {
  }
  intercept(request: HttpRequest<unknown>, next: HttpHandler):
    Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(tap(() => {
      console.log('Request was ok');
    }, (error: any) => {
      if (error instanceof HttpErrorResponse) {
        if (error.status === 401) {
          this.router.navigate(['login', { info: 'user not logged in anymore' }]);
        }
      }
    })
    );
  }
}
