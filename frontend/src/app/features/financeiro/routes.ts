import { Routes } from '@angular/router';

export const financeiroRoutes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./financeiro-page.component').then(m => m.FinanceiroPageComponent)
  }
];
