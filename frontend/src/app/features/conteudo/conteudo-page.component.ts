import { Component, inject, OnInit, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { ConteudoHttpService } from './conteudo.service';
import { ConteudoItemDto } from '../../core/models/conteudo.model';

@Component({
  selector: 'app-conteudo-page',
  standalone: true,
  imports: [
    DatePipe,
    MatButtonModule, MatCardModule, MatIconModule, MatProgressSpinnerModule
  ],
  template: `
    <div class="page-header">
      <div>
        <h1>Conteúdo</h1>
        <p class="subtitle">Artigos recentes de .NET e Angular</p>
      </div>
      <button mat-flat-button (click)="carregar()" [disabled]="loading()">
        <mat-icon>refresh</mat-icon>
        Atualizar
      </button>
    </div>

    @if (loading()) {
      <div class="loading-center"><mat-spinner diameter="40" /></div>
    }

    <div class="artigos-grid">
      @for (item of artigos(); track item.link) {
        <mat-card class="artigo-card">
          <mat-card-header>
            <mat-card-title class="artigo-titulo">{{ item.titulo }}</mat-card-title>
            <mat-card-subtitle>
              <span class="fonte-badge">{{ item.fonte }}</span>
              @if (item.publicadoEm) {
                · {{ item.publicadoEm | date:'dd/MM/yyyy' }}
              }
            </mat-card-subtitle>
          </mat-card-header>
          @if (item.resumo) {
            <mat-card-content>
              <p class="artigo-resumo">{{ item.resumo }}</p>
            </mat-card-content>
          }
          <mat-card-actions>
            <a mat-button color="primary" [href]="item.link" target="_blank" rel="noopener noreferrer">
              <mat-icon>open_in_new</mat-icon>
              Ler artigo
            </a>
          </mat-card-actions>
        </mat-card>
      } @empty {
        @if (!loading()) {
          <p class="vazio">Nenhum artigo disponível no momento.</p>
        }
      }
    </div>
  `,
  styles: [`
    .page-header { display: flex; justify-content: space-between; align-items: flex-start; margin-bottom: 24px; }
    h1 { margin: 0; }
    .subtitle { margin: 4px 0 0; color: var(--mat-sys-on-surface-variant); font-size: 0.875rem; }
    .loading-center { display: flex; justify-content: center; padding: 48px; }
    .artigos-grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(320px, 1fr)); gap: 16px; }
    .artigo-card { height: fit-content; display: flex; flex-direction: column; }
    .artigo-titulo { font-size: 0.95rem !important; line-height: 1.4 !important; }
    .fonte-badge {
      background: var(--mat-sys-secondary-container);
      color: var(--mat-sys-on-secondary-container);
      padding: 2px 8px;
      border-radius: 12px;
      font-size: 0.75rem;
    }
    .artigo-resumo {
      font-size: 0.875rem;
      color: var(--mat-sys-on-surface-variant);
      display: -webkit-box;
      -webkit-line-clamp: 3;
      -webkit-box-orient: vertical;
      overflow: hidden;
    }
    .vazio { color: var(--mat-sys-on-surface-variant); }
  `]
})
export class ConteudoPageComponent implements OnInit {
  private service = inject(ConteudoHttpService);

  artigos = signal<ConteudoItemDto[]>([]);
  loading = signal(false);

  ngOnInit(): void { this.carregar(); }

  carregar(): void {
    this.loading.set(true);
    this.service.getAll().subscribe({
      next: data => { this.artigos.set(data); this.loading.set(false); },
      error: () => this.loading.set(false)
    });
  }
}
