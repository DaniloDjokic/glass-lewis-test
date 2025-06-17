import { Routes } from '@angular/router';
import { LoginComponent } from './login/login';
import { CompanyListComponent } from './companylist/company.list';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  {
    path: 'companies',
    component: CompanyListComponent,
    canActivate: [authGuard]
  },
  { path: '', redirectTo: '/login', pathMatch: 'full' }, // Changed this
  { path: '**', redirectTo: '/login' } // Changed this
];
