import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { forkJoin, Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { FluxoMensalDto } from '../../core/models/financeiro.model';

@Injectable({ providedIn: 'root' })
export class FluxoCaixaService {
  private readonly http = inject(HttpClient);
  private readonly api = `${environment.apiUrl}/v1/fluxo-caixa`;

  getMes(ano: number, mes: number): Observable<FluxoMensalDto> {
    return this.http.get<FluxoMensalDto>(`${this.api}/${ano}/${mes}`);
  }

  getAno(ano: number): Observable<FluxoMensalDto[]> {
    return forkJoin(
      Array.from({ length: 12 }, (_, i) => this.getMes(ano, i + 1))
    );
  }
}
