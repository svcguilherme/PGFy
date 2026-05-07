---
name: angular-conventions
description: Use this skill when creating Angular components, services, interceptors, guards, or any frontend code. Trigger on: "componente", "serviço angular", "interceptor", "guard", "rota", "observable", "signal", "angular".
---

# Angular Conventions — CareerHub

## Standalone Component Pattern

```typescript
// features/estudos/components/estudo-card.component.ts
@Component({
  selector: 'app-estudo-card',
  standalone: true,
  imports: [CommonModule, RouterLink, MatCardModule],
  template: `
    <mat-card>
      <mat-card-title>{{ estudo().titulo }}</mat-card-title>
      <mat-card-subtitle>
        {{ estudo().diaDaSemana }} — {{ estudo().horasTotais }}h
      </mat-card-subtitle>
    </mat-card>
  `
})
export class EstudoCardComponent {
  estudo = input.required<EstudoDto>();
  editar = output<Guid>();
}
```

## Service Pattern (HTTP)

```typescript
// features/estudos/services/estudos.service.ts
@Injectable({ providedIn: 'root' })
export class EstudosService {
  private readonly api = `${environment.apiUrl}/v1/estudos`;
  private readonly http = inject(HttpClient);

  getAll(): Observable<EstudoDto[]> {
    return this.http.get<EstudoDto[]>(this.api);
  }

  getByDia(dia: DiaDaSemana): Observable<EstudoDto[]> {
    return this.http.get<EstudoDto[]>(`${this.api}?dia=${dia}`);
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
```

## Auth Interceptor

```typescript
// core/interceptors/auth.interceptor.ts
export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const token = authService.accessToken();

  if (token) {
    req = req.clone({
      setHeaders: { Authorization: `Bearer ${token}` }
    });
  }

  return next(req).pipe(
    catchError(err => {
      if (err.status === 401) {
        return authService.refresh().pipe(
          switchMap(newToken => {
            return next(req.clone({
              setHeaders: { Authorization: `Bearer ${newToken}` }
            }));
          }),
          catchError(() => {
            authService.logout();
            return throwError(() => err);
          })
        );
      }
      return throwError(() => err);
    })
  );
};
```

## Interfaces TypeScript (espelham DTOs do backend)

```typescript
// core/models/estudo.model.ts
export interface EstudoDto {
  id: string;
  titulo: string;
  diaDaSemana: DiaDaSemana;
  horaInicio: string;   // "HH:mm"
  horaFim: string;      // "HH:mm"
  horasTotais: number;
  descricao?: string;
}

export type DiaDaSemana =
  'Segunda' | 'Terca' | 'Quarta' | 'Quinta' | 'Sexta' | 'Sabado' | 'Domingo';

// core/models/financeiro.model.ts
export interface FluxoMensalDto {
  ano: number;
  mes: number;
  nomeMes: string;
  totalRecebiveis: number;
  totalDespesas: number;
  saldo: number;
  saldoPositivo: boolean;
  despesas: DespesaResumoDto[];
  recebiveis: RecebivelResumoDto[];
}
```

## Lazy Routes Pattern

```typescript
// app.routes.ts
export const routes: Routes = [
  { path: '', redirectTo: 'estudos', pathMatch: 'full' },
  { path: 'login', loadComponent: () => import('./features/auth/login.component') },
  {
    path: '',
    component: LayoutComponent,
    canActivate: [authGuard],
    children: [
      { path: 'estudos', loadChildren: () => import('./features/estudos/routes') },
      { path: 'financeiro', loadChildren: () => import('./features/financeiro/routes') },
      { path: 'posts', loadChildren: () => import('./features/posts/routes') },
      { path: 'conteudo', loadChildren: () => import('./features/conteudo/routes') },
    ]
  }
];
```
