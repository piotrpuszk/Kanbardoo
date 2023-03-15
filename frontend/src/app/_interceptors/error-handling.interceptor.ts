import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { ToastService } from '../_services/toast.service';
import { Router } from '@angular/router';
import { UsersService } from '../_services/users.service';

@Injectable()
export class ErrorHandlingInterceptor implements HttpInterceptor {

  constructor(private router: Router, 
    private usersService: UsersService,
    private toast: ToastService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(catchError((error: HttpErrorResponse) => {
      return this.handleErrors(error);
    }));
  }

  private handleErrors(error: HttpErrorResponse) {
    if(error.status === 401) {
      return this.handleUnauthenticated(error);
    }

    if(error.status === 403) {
      return this.handleUnauthorized(error);
    }

    return this.handleInternalServerError(error);
  }

  private handleInternalServerError(error: HttpErrorResponse) {
    this.toast.error("Internal server error");
    return throwError(() => error);
  }

  private handleUnauthenticated(error: HttpErrorResponse) {
    this.usersService.signOut();
    this.router.navigate(['/sign-in']);
    return throwError(() => error);
  }

  private handleUnauthorized(error: HttpErrorResponse) {
    if (!error.error) {
      this.usersService.signOut();
      this.router.navigate(['/sign-in']);
    }
    else {
      this.toast.error("Unauthorized");
      this.router.navigate(['/dashboard']);
    }
    return throwError(() => error);
  }
}
