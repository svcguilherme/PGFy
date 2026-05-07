import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { AuthService } from '../../core/auth/auth.service';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [MatButtonModule, MatCardModule, MatIconModule],
  template: `
    <div class="profile-container">
      <mat-card class="profile-card">
        <mat-card-header>
          <div mat-card-avatar class="avatar-icon">
            <mat-icon>account_circle</mat-icon>
          </div>
          <mat-card-title>{{ user()?.nome }}</mat-card-title>
          <mat-card-subtitle>{{ user()?.email }}</mat-card-subtitle>
        </mat-card-header>

        <mat-card-content>
          <div class="info-row">
            <mat-icon class="info-icon">badge</mat-icon>
            <div>
              <p class="info-label">ID</p>
              <p class="info-value">{{ user()?.id }}</p>
            </div>
          </div>
        </mat-card-content>

        <mat-card-actions align="end">
          <button mat-flat-button color="warn" (click)="logout()">
            <mat-icon>logout</mat-icon>
            Sair
          </button>
        </mat-card-actions>
      </mat-card>
    </div>
  `,
  styles: [`
    .profile-container {
      display: flex;
      justify-content: center;
      padding-top: 32px;
    }
    .profile-card { width: 100%; max-width: 480px; }
    .avatar-icon mat-icon { font-size: 40px; width: 40px; height: 40px; color: var(--mat-sys-primary); }
    .info-row { display: flex; align-items: flex-start; gap: 12px; padding: 16px 0; }
    .info-icon { color: var(--mat-sys-on-surface-variant); margin-top: 4px; }
    .info-label { margin: 0; font-size: 0.75rem; color: var(--mat-sys-on-surface-variant); }
    .info-value { margin: 2px 0 0; font-size: 0.875rem; word-break: break-all; }
  `]
})
export class ProfileComponent {
  private auth = inject(AuthService);
  private router = inject(Router);

  readonly user = this.auth.currentUser;

  logout(): void {
    this.auth.logout();
    this.router.navigate(['/login']);
  }
}
