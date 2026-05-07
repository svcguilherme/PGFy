import { Component, inject } from '@angular/core';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatTooltipModule } from '@angular/material/tooltip';
import { AuthService } from '../core/auth/auth.service';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [
    RouterOutlet, RouterLink, RouterLinkActive,
    MatToolbarModule, MatSidenavModule, MatListModule,
    MatIconModule, MatButtonModule, MatTooltipModule
  ],
  template: `
    <mat-sidenav-container class="sidenav-container">
      <mat-sidenav mode="side" opened class="sidenav">
        <div class="sidenav-header">
          <span class="app-name">CareerHub</span>
        </div>
        <mat-nav-list>
          <a mat-list-item routerLink="/estudos" routerLinkActive="active-link">
            <mat-icon matListItemIcon>school</mat-icon>
            <span matListItemTitle>Estudos</span>
          </a>
          <a mat-list-item routerLink="/financeiro" routerLinkActive="active-link">
            <mat-icon matListItemIcon>account_balance_wallet</mat-icon>
            <span matListItemTitle>Financeiro</span>
          </a>
          <a mat-list-item routerLink="/posts" routerLinkActive="active-link">
            <mat-icon matListItemIcon>article</mat-icon>
            <span matListItemTitle>Posts</span>
          </a>
          <a mat-list-item routerLink="/conteudo" routerLinkActive="active-link">
            <mat-icon matListItemIcon>feed</mat-icon>
            <span matListItemTitle>Conteúdo</span>
          </a>
          <a mat-list-item routerLink="/perfil" routerLinkActive="active-link">
            <mat-icon matListItemIcon>account_circle</mat-icon>
            <span matListItemTitle>Perfil</span>
          </a>
        </mat-nav-list>
      </mat-sidenav>

      <mat-sidenav-content>
        <mat-toolbar color="primary">
          <span class="toolbar-spacer"></span>
          @if (auth.currentUser(); as user) {
            <span class="toolbar-user">{{ user.nome }}</span>
          }
          <button mat-icon-button routerLink="/perfil" matTooltip="Meu Perfil">
            <mat-icon>account_circle</mat-icon>
          </button>
        </mat-toolbar>
        <main class="page-content">
          <router-outlet />
        </main>
      </mat-sidenav-content>
    </mat-sidenav-container>
  `,
  styles: [`
    .sidenav-container { height: 100vh; }
    .sidenav { width: 220px; border-right: 1px solid rgba(0,0,0,0.12); }
    .sidenav-header {
      padding: 20px 16px 8px;
      font-size: 1.25rem;
      font-weight: 600;
      color: var(--mat-sys-primary);
    }
    .toolbar-spacer { flex: 1 1 auto; }
    .toolbar-user { font-size: 0.875rem; margin-right: 8px; opacity: 0.9; }
    .page-content { padding: 24px; }
    .active-link { background: var(--mat-sys-secondary-container); }
  `]
})
export class LayoutComponent {
  readonly auth = inject(AuthService);
  private readonly router = inject(Router);
}
