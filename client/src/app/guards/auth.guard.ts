import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { map } from 'rxjs/operators';

export const authGuard = () => {
  const authService = inject(AuthService);
  const router = inject(Router);

  return authService.token$.pipe(
    map(token => {
      if (token) {
        return true;
      } else {
        router.navigate(['/login']);
        return false;
      }
    })
  );
};
