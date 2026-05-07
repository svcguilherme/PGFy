import { Routes } from '@angular/router';

export const postsRoutes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./posts-page.component').then(m => m.PostsPageComponent)
  }
];
