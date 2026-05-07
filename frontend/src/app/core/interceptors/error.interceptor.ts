import { inject } from '@angular/core';
import { HttpInterceptorFn } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';
import { Router } from '@angular/router';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);

  return next(req).pipe(
    catchError(err => {
      if (err.status === 403) {
        router.navigate(['/estudos']);
      }
      if (err.status === 0) {
        console.error('Sem conexão com o servidor.');
      }
      return throwError(() => err);
    })
  );
};
