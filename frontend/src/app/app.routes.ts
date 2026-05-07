import { Routes } from '@angular/router';
import { authGuard } from './core/auth/auth.guard';
import { LayoutComponent } from './layout/layout.component';

export const routes: Routes = [
  { path: '', redirectTo: 'estudos', pathMatch: 'full' },
  {
    path: 'login',
    loadComponent: () => import('./features/auth/login.component').then(m => m.LoginComponent)
  },
  {
    path: 'register',
    loadComponent: () => import('./features/auth/register.component').then(m => m.RegisterComponent)
  },
  {
    path: '',
    component: LayoutComponent,
    canActivate: [authGuard],
    children: [
      {
        path: 'estudos',
        loadChildren: () => import('./features/estudos/routes').then(m => m.estudosRoutes)
      },
      {
        path: 'financeiro',
        loadChildren: () => import('./features/financeiro/routes').then(m => m.financeiroRoutes)
      },
      {
        path: 'posts',
        loadChildren: () => import('./features/posts/routes').then(m => m.postsRoutes)
      },
      {
        path: 'conteudo',
        loadChildren: () => import('./features/conteudo/routes').then(m => m.conteudoRoutes)
      },
      {
        path: 'perfil',
        loadComponent: () => import('./features/auth/profile.component').then(m => m.ProfileComponent)
      }
    ]
  },
  { path: '**', redirectTo: 'estudos' }
];
