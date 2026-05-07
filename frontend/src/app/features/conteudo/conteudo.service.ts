import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ConteudoItemDto } from '../../core/models/conteudo.model';

@Injectable({ providedIn: 'root' })
export class ConteudoHttpService {
  private readonly http = inject(HttpClient);
  private readonly api = `${environment.apiUrl}/v1/conteudo`;

  getAll(): Observable<ConteudoItemDto[]> {
    return this.http.get<ConteudoItemDto[]>(this.api);
  }
}
