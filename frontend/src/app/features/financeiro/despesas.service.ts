import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { CreateDespesaDto, DespesaDto, UpdateDespesaDto } from '../../core/models/financeiro.model';

@Injectable({ providedIn: 'root' })
export class DespesasService {
  private readonly http = inject(HttpClient);
  private readonly api = `${environment.apiUrl}/v1/despesas`;

  getAll(): Observable<DespesaDto[]> {
    return this.http.get<DespesaDto[]>(this.api);
  }

  create(dto: CreateDespesaDto): Observable<DespesaDto> {
    return this.http.post<DespesaDto>(this.api, dto);
  }

  update(id: string, dto: UpdateDespesaDto): Observable<DespesaDto> {
    return this.http.put<DespesaDto>(`${this.api}/${id}`, dto);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.api}/${id}`);
  }

  marcarComoPago(id: string): Observable<DespesaDto> {
    return this.http.patch<DespesaDto>(`${this.api}/${id}/pago`, {});
  }
}
