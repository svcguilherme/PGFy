import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { CreateEstudoDto, EstudoDto, UpdateEstudoDto } from '../../core/models/estudo.model';

@Injectable({ providedIn: 'root' })
export class EstudosService {
  private readonly http = inject(HttpClient);
  private readonly api = `${environment.apiUrl}/v1/estudos`;

  getAll(): Observable<EstudoDto[]> {
    return this.http.get<EstudoDto[]>(this.api);
  }

  create(dto: CreateEstudoDto): Observable<EstudoDto> {
    return this.http.post<EstudoDto>(this.api, dto);
  }

  update(id: string, dto: UpdateEstudoDto): Observable<EstudoDto> {
    return this.http.put<EstudoDto>(`${this.api}/${id}`, dto);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.api}/${id}`);
  }
}
