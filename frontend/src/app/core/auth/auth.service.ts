import { computed, inject, Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AuthResponseDto, CurrentUser, LoginDto, RegisterDto } from '../models/auth.model';

// ClaimTypes URIs usados pelo .NET
const CLAIM_ID = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier';
const CLAIM_NAME = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name';
const CLAIM_EMAIL = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly _accessToken = signal<string | null>(null);
  readonly accessToken = this._accessToken.asReadonly();
  readonly isAuthenticated = computed(() => !!this._accessToken());

  readonly currentUser = computed<CurrentUser | null>(() => {
    const token = this._accessToken();
    if (!token) return null;
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return { id: payload[CLAIM_ID], nome: payload[CLAIM_NAME], email: payload[CLAIM_EMAIL] };
    } catch {
      return null;
    }
  });

  private readonly http = inject(HttpClient);
  private readonly api = `${environment.apiUrl}/v1/auth`;

  login(dto: LoginDto): Observable<AuthResponseDto> {
    return this.http.post<AuthResponseDto>(`${this.api}/login`, dto).pipe(
      tap(res => this.storeTokens(res))
    );
  }

  register(dto: RegisterDto): Observable<AuthResponseDto> {
    return this.http.post<AuthResponseDto>(`${this.api}/register`, dto).pipe(
      tap(res => this.storeTokens(res))
    );
  }

  refresh(): Observable<string> {
    const refreshToken = localStorage.getItem('refreshToken');
    return this.http.post<AuthResponseDto>(`${this.api}/refresh`, { refreshToken }).pipe(
      tap(res => this.storeTokens(res)),
      map(res => res.accessToken)
    );
  }

  logout(): void {
    this._accessToken.set(null);
    localStorage.removeItem('refreshToken');
  }

  private storeTokens(res: AuthResponseDto): void {
    this._accessToken.set(res.accessToken);
    localStorage.setItem('refreshToken', res.refreshToken);
  }
}
