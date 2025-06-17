import { Routes } from '@angular/router';
import { LoginComponent } from './login/login';
import { CompanyListComponent } from './company-list/company-list';
import { authGuard } from './guards/auth-guard';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  {
    path: 'companies',
    component: CompanyListComponent,
    canActivate: [authGuard]
  },
  { path: '', redirectTo: '/companies', pathMatch: 'full' },
  { path: '**', redirectTo: '/companies' }
];
