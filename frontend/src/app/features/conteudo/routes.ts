import { Routes } from '@angular/router';

export const conteudoRoutes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./conteudo-page.component').then(m => m.ConteudoPageComponent)
  }
];
