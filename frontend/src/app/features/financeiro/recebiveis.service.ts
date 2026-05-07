import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { CreateRecebivelDto, RecebivelDto, UpdateRecebivelDto } from '../../core/models/financeiro.model';

@Injectable({ providedIn: 'root' })
export class ReceiveisService {
  private readonly http = inject(HttpClient);
  private readonly api = `${environment.apiUrl}/v1/recebiveis`;

  getAll(): Observable<RecebivelDto[]> {
    return this.http.get<RecebivelDto[]>(this.api);
  }

  create(dto: CreateRecebivelDto): Observable<RecebivelDto> {
    return this.http.post<RecebivelDto>(this.api, dto);
  }

  update(id: string, dto: UpdateRecebivelDto): Observable<RecebivelDto> {
    return this.http.put<RecebivelDto>(`${this.api}/${id}`, dto);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.api}/${id}`);
  }

  marcarComoRecebido(id: string): Observable<RecebivelDto> {
    return this.http.patch<RecebivelDto>(`${this.api}/${id}/recebido`, {});
  }
}
