import { Routes } from '@angular/router';

export const estudosRoutes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./estudos-page.component').then(m => m.EstudosPageComponent)
  }
];
