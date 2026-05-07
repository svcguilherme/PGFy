import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { CreatePostDto, PostDto, UpdatePostDto } from '../../core/models/post.model';

@Injectable({ providedIn: 'root' })
export class PostsService {
  private readonly http = inject(HttpClient);
  private readonly api = `${environment.apiUrl}/v1/posts`;

  getAll(): Observable<PostDto[]> {
    return this.http.get<PostDto[]>(this.api);
  }

  create(dto: CreatePostDto): Observable<PostDto> {
    return this.http.post<PostDto>(this.api, dto);
  }

  update(id: string, dto: UpdatePostDto): Observable<PostDto> {
    return this.http.put<PostDto>(`${this.api}/${id}`, dto);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.api}/${id}`);
  }
}
