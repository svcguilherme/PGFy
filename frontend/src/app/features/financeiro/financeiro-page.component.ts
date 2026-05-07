import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { MatTabsModule } from '@angular/material/tabs';
import { MatTooltipModule } from '@angular/material/tooltip';
import { DespesasService } from './despesas.service';
import { ReceiveisService } from './recebiveis.service';
import { FluxoCaixaService } from './fluxo-caixa.service';
import {
  CATEGORIAS_DESPESA, CategoriaDespesa,
  CreateDespesaDto, CreateRecebivelDto,
  DespesaDto, FluxoMensalDto, RecebivelDto,
  UpdateDespesaDto, UpdateRecebivelDto
} from '../../core/models/financeiro.model';

const MESES = ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'];

@Component({
  selector: 'app-financeiro-page',
  standalone: true,
  imports: [
    CurrencyPipe, DatePipe, ReactiveFormsModule,
    MatButtonModule, MatCardModule, MatDividerModule, MatFormFieldModule,
    MatIconModule, MatInputModule, MatProgressSpinnerModule,
    MatSelectModule, MatTabsModule, MatTooltipModule
  ],
  template: `
    <h1>Financeiro</h1>

    <mat-tab-group>

      <!-- ───── DESPESAS ───── -->
      <mat-tab label="Despesas">
        <div class="tab-content">
          <div class="tab-header">
            <span class="total-label">
              Pendente: {{ totalDespesas() | currency:'BRL':'symbol':'1.2-2' }}
            </span>
            <button mat-flat-button color="primary" (click)="abrirFormDespesa()">
              <mat-icon>add</mat-icon> Nova Despesa
            </button>
          </div>

          @if (loadingDespesas()) {
            <div class="loading-center"><mat-spinner diameter="36" /></div>
          }

          @if (formDespesaAberto()) {
            <mat-card class="form-card">
              <mat-card-header>
                <mat-card-title>{{ editandoDespesaId() ? 'Editar Despesa' : 'Nova Despesa' }}</mat-card-title>
              </mat-card-header>
              <mat-card-content>
                <form [formGroup]="formDespesa" class="form-grid">
                  <mat-form-field appearance="outline" class="span-2">
                    <mat-label>Descrição</mat-label>
                    <input matInput formControlName="descricao" />
                  </mat-form-field>
                  <mat-form-field appearance="outline">
                    <mat-label>Valor (R$)</mat-label>
                    <input matInput formControlName="valor" type="number" min="0" step="0.01" />
                  </mat-form-field>
                  <mat-form-field appearance="outline">
                    <mat-label>Data</mat-label>
                    <input matInput formControlName="dataPrevista" type="date" />
                  </mat-form-field>
                  <mat-form-field appearance="outline" class="span-2">
                    <mat-label>Categoria</mat-label>
                    <mat-select formControlName="categoria">
                      @for (cat of categorias; track cat) {
                        <mat-option [value]="cat">{{ cat }}</mat-option>
                      }
                    </mat-select>
                  </mat-form-field>
                </form>
              </mat-card-content>
              <mat-card-actions align="end">
                <button mat-button (click)="fecharFormDespesa()">Cancelar</button>
                <button mat-flat-button color="primary" (click)="salvarDespesa()"
                        [disabled]="formDespesa.invalid || salvandoDespesa()">
                  {{ salvandoDespesa() ? 'Salvando...' : 'Salvar' }}
                </button>
              </mat-card-actions>
            </mat-card>
          }

          <div class="itens-lista">
            @for (d of despesas(); track d.id) {
              <mat-card class="item-card" [class.item-quitado]="d.pago">
                <mat-card-content>
                  <div class="item-row">
                    <div class="item-info">
                      <span class="item-titulo">{{ d.descricao }}</span>
                      <span class="item-meta">
                        {{ d.dataPrevista | date:'dd/MM/yyyy' }} · {{ d.categoria }}
                        @if (d.pago) { · <strong>Pago</strong> }
                      </span>
                    </div>
                    <div class="item-right">
                      <span class="item-valor" [class.valor-quitado]="d.pago">
                        {{ d.valor | currency:'BRL':'symbol':'1.2-2' }}
                      </span>
                      <div class="item-actions">
                        @if (!d.pago) {
                          <button mat-icon-button color="accent"
                                  (click)="marcarDespesaPago(d.id)"
                                  matTooltip="Marcar como pago">
                            <mat-icon>check_circle</mat-icon>
                          </button>
                        }
                        <button mat-icon-button (click)="abrirFormEditarDespesa(d)" matTooltip="Editar">
                          <mat-icon>edit</mat-icon>
                        </button>
                        <button mat-icon-button color="warn" (click)="deletarDespesa(d.id)" matTooltip="Excluir">
                          <mat-icon>delete</mat-icon>
                        </button>
                      </div>
                    </div>
                  </div>
                </mat-card-content>
              </mat-card>
            } @empty {
              @if (!loadingDespesas()) {
                <p class="vazio">Nenhuma despesa registrada.</p>
              }
            }
          </div>
        </div>
      </mat-tab>

      <!-- ───── RECEBÍVEIS ───── -->
      <mat-tab label="Recebíveis">
        <div class="tab-content">
          <div class="tab-header">
            <span class="total-label">
              Pendente: {{ totalRecebiveis() | currency:'BRL':'symbol':'1.2-2' }}
            </span>
            <button mat-flat-button color="primary" (click)="abrirFormRecebivel()">
              <mat-icon>add</mat-icon> Novo Recebível
            </button>
          </div>

          @if (loadingRecebiveis()) {
            <div class="loading-center"><mat-spinner diameter="36" /></div>
          }

          @if (formRecebivelAberto()) {
            <mat-card class="form-card">
              <mat-card-header>
                <mat-card-title>{{ editandoRecebivelId() ? 'Editar Recebível' : 'Novo Recebível' }}</mat-card-title>
              </mat-card-header>
              <mat-card-content>
                <form [formGroup]="formRecebivel" class="form-grid">
                  <mat-form-field appearance="outline" class="span-2">
                    <mat-label>Descrição</mat-label>
                    <input matInput formControlName="descricao" />
                  </mat-form-field>
                  <mat-form-field appearance="outline">
                    <mat-label>Valor Previsto (R$)</mat-label>
                    <input matInput formControlName="valorPrevisto" type="number" min="0" step="0.01" />
                  </mat-form-field>
                  <mat-form-field appearance="outline">
                    <mat-label>Data Prevista</mat-label>
                    <input matInput formControlName="dataPrevista" type="date" />
                  </mat-form-field>
                </form>
              </mat-card-content>
              <mat-card-actions align="end">
                <button mat-button (click)="fecharFormRecebivel()">Cancelar</button>
                <button mat-flat-button color="primary" (click)="salvarRecebivel()"
                        [disabled]="formRecebivel.invalid || salvandoRecebivel()">
                  {{ salvandoRecebivel() ? 'Salvando...' : 'Salvar' }}
                </button>
              </mat-card-actions>
            </mat-card>
          }

          <div class="itens-lista">
            @for (r of recebiveis(); track r.id) {
              <mat-card class="item-card" [class.item-quitado]="r.recebido">
                <mat-card-content>
                  <div class="item-row">
                    <div class="item-info">
                      <span class="item-titulo">{{ r.descricao }}</span>
                      <span class="item-meta">
                        {{ r.dataPrevista | date:'dd/MM/yyyy' }}
                        @if (r.recebido) { · <strong>Recebido</strong> }
                      </span>
                    </div>
                    <div class="item-right">
                      <span class="item-valor" [class.valor-quitado]="r.recebido">
                        {{ r.valorPrevisto | currency:'BRL':'symbol':'1.2-2' }}
                      </span>
                      <div class="item-actions">
                        @if (!r.recebido) {
                          <button mat-icon-button color="accent"
                                  (click)="marcarRecebivelRecebido(r.id)"
                                  matTooltip="Marcar como recebido">
                            <mat-icon>check_circle</mat-icon>
                          </button>
                        }
                        <button mat-icon-button (click)="abrirFormEditarRecebivel(r)" matTooltip="Editar">
                          <mat-icon>edit</mat-icon>
                        </button>
                        <button mat-icon-button color="warn" (click)="deletarRecebivel(r.id)" matTooltip="Excluir">
                          <mat-icon>delete</mat-icon>
                        </button>
                      </div>
                    </div>
                  </div>
                </mat-card-content>
              </mat-card>
            } @empty {
              @if (!loadingRecebiveis()) {
                <p class="vazio">Nenhum recebível registrado.</p>
              }
            }
          </div>
        </div>
      </mat-tab>

      <!-- ───── FLUXO DE CAIXA ───── -->
      <mat-tab label="Fluxo de Caixa">
        <div class="tab-content">
          <div class="tab-header">
            <span class="total-label">Resumo anual</span>
            <div class="ano-controls">
              <button mat-icon-button (click)="anoAnterior()">
                <mat-icon>chevron_left</mat-icon>
              </button>
              <span class="ano-label">{{ anoSelecionado() }}</span>
              <button mat-icon-button (click)="proximoAno()">
                <mat-icon>chevron_right</mat-icon>
              </button>
              <button mat-flat-button color="primary" (click)="carregarFluxo()">
                <mat-icon>refresh</mat-icon>
              </button>
            </div>
          </div>

          @if (loadingFluxo()) {
            <div class="loading-center"><mat-spinner diameter="36" /></div>
          }

          <div class="fluxo-grid">
            @for (mes of fluxoAno(); track mes.mes) {
              <mat-card class="fluxo-card" [class.fluxo-positivo]="mes.positivo" [class.fluxo-negativo]="!mes.positivo">
                <mat-card-header>
                  <mat-card-title class="mes-titulo">{{ nomeMes(mes.mes) }}</mat-card-title>
                </mat-card-header>
                <mat-card-content>
                  <div class="fluxo-row">
                    <span class="fluxo-label">Receitas</span>
                    <span class="valor-positivo">{{ mes.totalRecebiveis | currency:'BRL':'symbol':'1.2-2' }}</span>
                  </div>
                  <div class="fluxo-row">
                    <span class="fluxo-label">Despesas</span>
                    <span class="valor-negativo">{{ mes.totalDespesas | currency:'BRL':'symbol':'1.2-2' }}</span>
                  </div>
                  <mat-divider style="margin: 8px 0" />
                  <div class="fluxo-row">
                    <span class="fluxo-label"><strong>Saldo</strong></span>
                    <strong [class.valor-positivo]="mes.positivo" [class.valor-negativo]="!mes.positivo">
                      {{ mes.saldo | currency:'BRL':'symbol':'1.2-2' }}
                    </strong>
                  </div>
                </mat-card-content>
              </mat-card>
            } @empty {
              @if (!loadingFluxo()) {
                <p class="vazio">Nenhum dado de fluxo para {{ anoSelecionado() }}.</p>
              }
            }
          </div>
        </div>
      </mat-tab>

    </mat-tab-group>
  `,
  styles: [`
    h1 { margin: 0 0 16px; }
    .tab-content { padding: 16px 0; }
    .tab-header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 16px; flex-wrap: wrap; gap: 8px; }
    .total-label { font-size: 0.95rem; color: var(--mat-sys-on-surface-variant); }
    .loading-center { display: flex; justify-content: center; padding: 32px; }
    .form-card { margin-bottom: 16px; }
    .form-grid { display: grid; grid-template-columns: 1fr 1fr; gap: 0 16px; }
    .span-2 { grid-column: span 2; }

    .itens-lista { display: flex; flex-direction: column; gap: 8px; }
    .item-card { transition: opacity 0.2s; }
    .item-card.item-quitado { opacity: 0.55; }
    .item-row { display: flex; justify-content: space-between; align-items: center; gap: 8px; }
    .item-info { display: flex; flex-direction: column; min-width: 0; }
    .item-titulo { font-weight: 500; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }
    .item-meta { font-size: 0.8rem; color: var(--mat-sys-on-surface-variant); }
    .item-right { display: flex; align-items: center; gap: 4px; flex-shrink: 0; }
    .item-valor { font-weight: 600; font-size: 1rem; }
    .item-actions { display: flex; }
    .valor-quitado { text-decoration: line-through; opacity: 0.7; }

    .ano-controls { display: flex; align-items: center; gap: 4px; }
    .ano-label { font-size: 1.1rem; font-weight: 600; min-width: 48px; text-align: center; }
    .fluxo-grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(195px, 1fr)); gap: 12px; }
    .fluxo-card { height: fit-content; }
    .fluxo-card.fluxo-positivo { border-top: 3px solid var(--mat-sys-tertiary); }
    .fluxo-card.fluxo-negativo { border-top: 3px solid var(--mat-sys-error); }
    .mes-titulo { font-size: 1rem !important; }
    .fluxo-row { display: flex; justify-content: space-between; align-items: center; margin-bottom: 4px; font-size: 0.85rem; }
    .fluxo-label { color: var(--mat-sys-on-surface-variant); }
    .valor-positivo { color: var(--mat-sys-tertiary); }
    .valor-negativo { color: var(--mat-sys-error); }
    .vazio { color: var(--mat-sys-on-surface-variant); }
  `]
})
export class FinanceiroPageComponent implements OnInit {
  private despesasService = inject(DespesasService);
  private receiveisService = inject(ReceiveisService);
  private fluxoService = inject(FluxoCaixaService);

  readonly categorias = CATEGORIAS_DESPESA;

  // ── Despesas ──
  despesas = signal<DespesaDto[]>([]);
  loadingDespesas = signal(false);
  formDespesaAberto = signal(false);
  salvandoDespesa = signal(false);
  editandoDespesaId = signal<string | null>(null);

  totalDespesas = computed(() =>
    this.despesas().filter(d => !d.pago).reduce((acc, d) => acc + d.valor, 0)
  );

  formDespesa = new FormGroup({
    descricao: new FormControl('', Validators.required),
    valor: new FormControl<number | null>(null, [Validators.required, Validators.min(0.01)]),
    dataPrevista: new FormControl('', Validators.required),
    categoria: new FormControl<CategoriaDespesa | null>(null, Validators.required)
  });

  // ── Recebiveis ──
  recebiveis = signal<RecebivelDto[]>([]);
  loadingRecebiveis = signal(false);
  formRecebivelAberto = signal(false);
  salvandoRecebivel = signal(false);
  editandoRecebivelId = signal<string | null>(null);

  totalRecebiveis = computed(() =>
    this.recebiveis().filter(r => !r.recebido).reduce((acc, r) => acc + r.valorPrevisto, 0)
  );

  formRecebivel = new FormGroup({
    descricao: new FormControl('', Validators.required),
    valorPrevisto: new FormControl<number | null>(null, [Validators.required, Validators.min(0.01)]),
    dataPrevista: new FormControl('', Validators.required)
  });

  // ── Fluxo de Caixa ──
  anoSelecionado = signal(new Date().getFullYear());
  fluxoAno = signal<FluxoMensalDto[]>([]);
  loadingFluxo = signal(false);

  ngOnInit(): void {
    this.carregarDespesas();
    this.carregarRecebiveis();
    this.carregarFluxo();
  }

  nomeMes(mes: number): string {
    return MESES[mes - 1];
  }

  // ── Despesas ──

  carregarDespesas(): void {
    this.loadingDespesas.set(true);
    this.despesasService.getAll().subscribe({
      next: data => { this.despesas.set(data); this.loadingDespesas.set(false); },
      error: () => this.loadingDespesas.set(false)
    });
  }

  abrirFormDespesa(): void {
    this.editandoDespesaId.set(null);
    this.formDespesa.reset();
    this.formDespesaAberto.set(true);
  }

  abrirFormEditarDespesa(d: DespesaDto): void {
    this.editandoDespesaId.set(d.id);
    this.formDespesa.setValue({
      descricao: d.descricao,
      valor: d.valor,
      dataPrevista: d.dataPrevista.split('T')[0],
      categoria: d.categoria
    });
    this.formDespesaAberto.set(true);
  }

  fecharFormDespesa(): void {
    this.formDespesaAberto.set(false);
    this.editandoDespesaId.set(null);
    this.formDespesa.reset();
  }

  salvarDespesa(): void {
    if (this.formDespesa.invalid) return;
    this.salvandoDespesa.set(true);
    const v = this.formDespesa.value;
    const dto = {
      descricao: v.descricao!,
      valor: v.valor!,
      dataPrevista: v.dataPrevista!,
      categoria: v.categoria!
    };
    const id = this.editandoDespesaId();
    const req = id
      ? this.despesasService.update(id, dto as UpdateDespesaDto)
      : this.despesasService.create(dto as CreateDespesaDto);

    req.subscribe({
      next: () => { this.fecharFormDespesa(); this.carregarDespesas(); this.salvandoDespesa.set(false); },
      error: () => this.salvandoDespesa.set(false)
    });
  }

  marcarDespesaPago(id: string): void {
    this.despesasService.marcarComoPago(id).subscribe(() => this.carregarDespesas());
  }

  deletarDespesa(id: string): void {
    if (!confirm('Excluir esta despesa?')) return;
    this.despesasService.delete(id).subscribe(() => this.carregarDespesas());
  }

  // ── Recebiveis ──

  carregarRecebiveis(): void {
    this.loadingRecebiveis.set(true);
    this.receiveisService.getAll().subscribe({
      next: data => { this.recebiveis.set(data); this.loadingRecebiveis.set(false); },
      error: () => this.loadingRecebiveis.set(false)
    });
  }

  abrirFormRecebivel(): void {
    this.editandoRecebivelId.set(null);
    this.formRecebivel.reset();
    this.formRecebivelAberto.set(true);
  }

  abrirFormEditarRecebivel(r: RecebivelDto): void {
    this.editandoRecebivelId.set(r.id);
    this.formRecebivel.setValue({
      descricao: r.descricao,
      valorPrevisto: r.valorPrevisto,
      dataPrevista: r.dataPrevista.split('T')[0]
    });
    this.formRecebivelAberto.set(true);
  }

  fecharFormRecebivel(): void {
    this.formRecebivelAberto.set(false);
    this.editandoRecebivelId.set(null);
    this.formRecebivel.reset();
  }

  salvarRecebivel(): void {
    if (this.formRecebivel.invalid) return;
    this.salvandoRecebivel.set(true);
    const v = this.formRecebivel.value;
    const dto = {
      descricao: v.descricao!,
      valorPrevisto: v.valorPrevisto!,
      dataPrevista: v.dataPrevista!
    };
    const id = this.editandoRecebivelId();
    const req = id
      ? this.receiveisService.update(id, dto as UpdateRecebivelDto)
      : this.receiveisService.create(dto as CreateRecebivelDto);

    req.subscribe({
      next: () => { this.fecharFormRecebivel(); this.carregarRecebiveis(); this.salvandoRecebivel.set(false); },
      error: () => this.salvandoRecebivel.set(false)
    });
  }

  marcarRecebivelRecebido(id: string): void {
    this.receiveisService.marcarComoRecebido(id).subscribe(() => this.carregarRecebiveis());
  }

  deletarRecebivel(id: string): void {
    if (!confirm('Excluir este recebível?')) return;
    this.receiveisService.delete(id).subscribe(() => this.carregarRecebiveis());
  }

  // ── Fluxo de Caixa ──

  carregarFluxo(): void {
    this.loadingFluxo.set(true);
    this.fluxoService.getAno(this.anoSelecionado()).subscribe({
      next: data => { this.fluxoAno.set(data); this.loadingFluxo.set(false); },
      error: () => this.loadingFluxo.set(false)
    });
  }

  anoAnterior(): void {
    this.anoSelecionado.update(a => a - 1);
    this.carregarFluxo();
  }

  proximoAno(): void {
    this.anoSelecionado.update(a => a + 1);
    this.carregarFluxo();
  }
}
