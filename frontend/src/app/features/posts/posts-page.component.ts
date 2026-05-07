import { Component, inject, OnInit, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTooltipModule } from '@angular/material/tooltip';
import { PostsService } from './posts.service';
import { CreatePostDto, PostDto, UpdatePostDto } from '../../core/models/post.model';

@Component({
  selector: 'app-posts-page',
  standalone: true,
  imports: [
    DatePipe, ReactiveFormsModule,
    MatButtonModule, MatCardModule, MatFormFieldModule,
    MatIconModule, MatInputModule, MatProgressSpinnerModule, MatTooltipModule
  ],
  template: `
    <div class="page-header">
      <h1>Posts</h1>
      <button mat-fab extended color="primary" (click)="abrirFormCriar()">
        <mat-icon>add</mat-icon>
        Novo Post
      </button>
    </div>

    @if (loading()) {
      <div class="loading-center"><mat-spinner diameter="40" /></div>
    }

    @if (formAberto()) {
      <mat-card class="form-card">
        <mat-card-header>
          <mat-card-title>{{ editandoId() ? 'Editar Post' : 'Novo Post' }}</mat-card-title>
        </mat-card-header>
        <mat-card-content>
          <form [formGroup]="form" class="form-col">
            <mat-form-field appearance="outline">
              <mat-label>Título</mat-label>
              <input matInput formControlName="titulo" />
            </mat-form-field>
            <mat-form-field appearance="outline">
              <mat-label>Conteúdo</mat-label>
              <textarea matInput formControlName="conteudo" rows="8"></textarea>
            </mat-form-field>
          </form>
        </mat-card-content>
        <mat-card-actions align="end">
          <button mat-button (click)="fecharForm()">Cancelar</button>
          <button mat-flat-button color="primary" (click)="salvar()" [disabled]="form.invalid || salvando()">
            {{ salvando() ? 'Salvando...' : 'Salvar' }}
          </button>
        </mat-card-actions>
      </mat-card>
    }

    <div class="posts-grid">
      @for (post of posts(); track post.id) {
        <mat-card class="post-card">
          <mat-card-header>
            <mat-card-title>{{ post.titulo }}</mat-card-title>
            <mat-card-subtitle>{{ post.dataPublicacao | date:'dd/MM/yyyy' }}</mat-card-subtitle>
          </mat-card-header>
          <mat-card-content>
            <p class="post-preview">{{ post.conteudo }}</p>
          </mat-card-content>
          <mat-card-actions align="end">
            <button mat-icon-button (click)="abrirFormEditar(post)" matTooltip="Editar">
              <mat-icon>edit</mat-icon>
            </button>
            <button mat-icon-button color="warn" (click)="deletar(post.id)" matTooltip="Excluir">
              <mat-icon>delete</mat-icon>
            </button>
          </mat-card-actions>
        </mat-card>
      } @empty {
        @if (!loading()) {
          <p class="vazio">Nenhum post publicado ainda.</p>
        }
      }
    </div>
  `,
  styles: [`
    .page-header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 24px; }
    h1 { margin: 0; }
    .loading-center { display: flex; justify-content: center; padding: 48px; }
    .form-card { margin-bottom: 24px; }
    .form-col { display: flex; flex-direction: column; gap: 8px; padding-top: 8px; }
    .posts-grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(340px, 1fr)); gap: 16px; }
    .post-card { height: fit-content; }
    .post-preview {
      display: -webkit-box;
      -webkit-line-clamp: 4;
      -webkit-box-orient: vertical;
      overflow: hidden;
      white-space: pre-wrap;
    }
    .vazio { color: var(--mat-sys-on-surface-variant); }
  `]
})
export class PostsPageComponent implements OnInit {
  private service = inject(PostsService);

  posts = signal<PostDto[]>([]);
  loading = signal(false);
  formAberto = signal(false);
  salvando = signal(false);
  editandoId = signal<string | null>(null);

  form = new FormGroup({
    titulo: new FormControl('', [Validators.required, Validators.minLength(3)]),
    conteudo: new FormControl('', [Validators.required, Validators.minLength(10)])
  });

  ngOnInit(): void { this.carregar(); }

  carregar(): void {
    this.loading.set(true);
    this.service.getAll().subscribe({
      next: data => { this.posts.set(data); this.loading.set(false); },
      error: () => this.loading.set(false)
    });
  }

  abrirFormCriar(): void {
    this.editandoId.set(null);
    this.form.reset();
    this.formAberto.set(true);
  }

  abrirFormEditar(post: PostDto): void {
    this.editandoId.set(post.id);
    this.form.setValue({ titulo: post.titulo, conteudo: post.conteudo });
    this.formAberto.set(true);
  }

  fecharForm(): void {
    this.formAberto.set(false);
    this.editandoId.set(null);
    this.form.reset();
  }

  salvar(): void {
    if (this.form.invalid) return;
    this.salvando.set(true);
    const dto = this.form.value as CreatePostDto | UpdatePostDto;
    const id = this.editandoId();
    const req = id
      ? this.service.update(id, dto as UpdatePostDto)
      : this.service.create(dto as CreatePostDto);

    req.subscribe({
      next: () => { this.fecharForm(); this.carregar(); this.salvando.set(false); },
      error: () => this.salvando.set(false)
    });
  }

  deletar(id: string): void {
    if (!confirm('Excluir este post?')) return;
    this.service.delete(id).subscribe(() => this.carregar());
  }
}
