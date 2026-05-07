import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { DecimalPipe } from '@angular/common';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { MatTooltipModule } from '@angular/material/tooltip';
import { EstudosService } from './estudos.service';
import {
  CreateEstudoDto, DIAS_DA_SEMANA, DiaDaSemana,
  EstudoDto, UpdateEstudoDto
} from '../../core/models/estudo.model';

@Component({
  selector: 'app-estudos-page',
  standalone: true,
  imports: [
    DecimalPipe, ReactiveFormsModule,
    MatButtonModule, MatCardModule, MatDividerModule, MatFormFieldModule,
    MatIconModule, MatInputModule, MatProgressSpinnerModule, MatSelectModule, MatTooltipModule
  ],
  template: `
    <div class="page-header">
      <div>
        <h1>Estudos</h1>
        <p class="subtitle">{{ totalHorasSemana() | number:'1.0-1' }} horas esta semana</p>
      </div>
      <button mat-fab extended color="primary" (click)="abrirFormCriar()">
        <mat-icon>add</mat-icon>
        Novo Estudo
      </button>
    </div>

    @if (loading()) {
      <div class="loading-center"><mat-spinner diameter="40" /></div>
    }

    @if (formAberto()) {
      <mat-card class="form-card">
        <mat-card-header>
          <mat-card-title>{{ editandoId() ? 'Editar Estudo' : 'Novo Estudo' }}</mat-card-title>
        </mat-card-header>
        <mat-card-content>
          <form [formGroup]="form" class="form-grid">
            <mat-form-field appearance="outline">
              <mat-label>Título</mat-label>
              <input matInput formControlName="titulo" />
            </mat-form-field>
            <mat-form-field appearance="outline">
              <mat-label>Dia da Semana</mat-label>
              <mat-select formControlName="diaDaSemana">
                @for (dia of dias; track dia) {
                  <mat-option [value]="dia">{{ dia }}</mat-option>
                }
              </mat-select>
            </mat-form-field>
            <mat-form-field appearance="outline">
              <mat-label>Hora Início</mat-label>
              <input matInput formControlName="horaInicio" type="time" />
            </mat-form-field>
            <mat-form-field appearance="outline">
              <mat-label>Hora Fim</mat-label>
              <input matInput formControlName="horaFim" type="time" />
            </mat-form-field>
            <mat-form-field appearance="outline" class="span-2">
              <mat-label>Descrição (opcional)</mat-label>
              <textarea matInput formControlName="descricao" rows="2"></textarea>
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

    <div class="dias-grid">
      @for (dia of dias; track dia) {
        <mat-card class="dia-card">
          <mat-card-header>
            <mat-card-title>{{ dia }}</mat-card-title>
            <mat-card-subtitle>{{ horasDia(dia) | number:'1.0-1' }}h</mat-card-subtitle>
          </mat-card-header>
          <mat-card-content>
            @for (estudo of estudosPorDia()[dia]; track estudo.id) {
              <div class="estudo-item">
                <div class="estudo-info">
                  <span class="estudo-titulo">{{ estudo.titulo }}</span>
                  <span class="horario">
                    {{ estudo.horaInicio }} – {{ estudo.horaFim }}
                    ({{ estudo.horasTotais | number:'1.0-1' }}h)
                  </span>
                </div>
                <div>
                  <button mat-icon-button (click)="abrirFormEditar(estudo)" matTooltip="Editar">
                    <mat-icon>edit</mat-icon>
                  </button>
                  <button mat-icon-button color="warn" (click)="deletar(estudo.id)" matTooltip="Excluir">
                    <mat-icon>delete</mat-icon>
                  </button>
                </div>
              </div>
              <mat-divider />
            } @empty {
              <p class="vazio">Nenhum estudo</p>
            }
          </mat-card-content>
        </mat-card>
      }
    </div>
  `,
  styles: [`
    .page-header { display: flex; justify-content: space-between; align-items: flex-start; margin-bottom: 24px; }
    h1 { margin: 0; }
    .subtitle { margin: 4px 0 0; color: var(--mat-sys-on-surface-variant); font-size: 0.875rem; }
    .loading-center { display: flex; justify-content: center; padding: 48px; }
    .form-card { margin-bottom: 24px; }
    .form-grid { display: grid; grid-template-columns: 1fr 1fr; gap: 0 16px; }
    .span-2 { grid-column: span 2; }
    .dias-grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(260px, 1fr)); gap: 16px; }
    .dia-card { height: fit-content; }
    .vazio { color: var(--mat-sys-on-surface-variant); font-size: 0.875rem; margin: 8px 0; }
    .estudo-item { display: flex; justify-content: space-between; align-items: center; padding: 4px 0; }
    .estudo-info { display: flex; flex-direction: column; }
    .estudo-titulo { font-weight: 500; }
    .horario { font-size: 0.8rem; color: var(--mat-sys-on-surface-variant); }
  `]
})
export class EstudosPageComponent implements OnInit {
  private service = inject(EstudosService);

  readonly dias = DIAS_DA_SEMANA;
  estudos = signal<EstudoDto[]>([]);
  loading = signal(false);
  formAberto = signal(false);
  salvando = signal(false);
  editandoId = signal<string | null>(null);

  estudosPorDia = computed(() => {
    const map: Record<string, EstudoDto[]> = {};
    for (const dia of this.dias) map[dia] = [];
    for (const e of this.estudos()) map[e.diaDaSemana].push(e);
    return map as Record<DiaDaSemana, EstudoDto[]>;
  });

  totalHorasSemana = computed(() =>
    this.estudos().reduce((acc, e) => acc + e.horasTotais, 0)
  );

  form = new FormGroup({
    titulo: new FormControl('', [Validators.required, Validators.minLength(2)]),
    diaDaSemana: new FormControl<DiaDaSemana | null>(null, Validators.required),
    horaInicio: new FormControl('', Validators.required),
    horaFim: new FormControl('', Validators.required),
    descricao: new FormControl<string | null>(null)
  });

  ngOnInit(): void { this.carregar(); }

  carregar(): void {
    this.loading.set(true);
    this.service.getAll().subscribe({
      next: data => { this.estudos.set(data); this.loading.set(false); },
      error: () => this.loading.set(false)
    });
  }

  horasDia(dia: DiaDaSemana): number {
    return this.estudosPorDia()[dia].reduce((acc, e) => acc + e.horasTotais, 0);
  }

  abrirFormCriar(): void {
    this.editandoId.set(null);
    this.form.reset();
    this.formAberto.set(true);
  }

  abrirFormEditar(estudo: EstudoDto): void {
    this.editandoId.set(estudo.id);
    this.form.setValue({
      titulo: estudo.titulo,
      diaDaSemana: estudo.diaDaSemana,
      horaInicio: estudo.horaInicio,
      horaFim: estudo.horaFim,
      descricao: estudo.descricao ?? null
    });
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
    const { titulo, diaDaSemana, horaInicio, horaFim, descricao } = this.form.value;
    const dto = {
      titulo: titulo!,
      diaDaSemana: diaDaSemana!,
      horaInicio: horaInicio!,
      horaFim: horaFim!,
      descricao: descricao ?? undefined
    };
    const id = this.editandoId();
    const req = id
      ? this.service.update(id, dto as UpdateEstudoDto)
      : this.service.create(dto as CreateEstudoDto);

    req.subscribe({
      next: () => { this.fecharForm(); this.carregar(); this.salvando.set(false); },
      error: () => this.salvando.set(false)
    });
  }

  deletar(id: string): void {
    if (!confirm('Excluir este estudo?')) return;
    this.service.delete(id).subscribe(() => this.carregar());
  }
}
