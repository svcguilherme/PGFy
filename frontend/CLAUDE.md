# Frontend — CareerHub Angular

> Herda todas as regras de /CLAUDE.md

## Stack e Versões
- Angular 18 (Standalone Components)
- TypeScript 5.4+
- Angular Material ou PrimeNG (TBD)
- RxJS 7+ | Angular Signals para estado local
- JWT decode client-side
- SCSS para estilos

## Build e execução
```bash
cd frontend
npm install
ng serve                  # dev
ng build --configuration=production  # prod
ng test                   # unit tests
```

## Estrutura de Pastas

```
src/
├── app/
│   ├── core/
│   │   ├── auth/           → AuthService, JwtInterceptor, AuthGuard
│   │   ├── interceptors/   → ErrorInterceptor, LoadingInterceptor
│   │   └── models/         → interfaces TypeScript (espelham DTOs do backend)
│   ├── features/
│   │   ├── auth/           → login, register, profile
│   │   ├── estudos/        → listagem semanal, CRUD
│   │   ├── financeiro/     → despesas, recebiveis, fluxo de caixa
│   │   ├── posts/          → CRUD posts
│   │   └── conteudo/       → feed de artigos (readonly)
│   ├── shared/
│   │   ├── components/     → componentes reutilizáveis
│   │   └── pipes/          → pipes customizados
│   └── layout/             → navbar, sidebar, layout base
├── environments/
└── styles/                 → variáveis SCSS globais
```

## Padrões obrigatórios no frontend

### Autenticação JWT
- Access Token: armazenar em memória (variável no AuthService) — NÃO em localStorage
- Refresh Token: cookie HttpOnly (ou localStorage se não tiver backend proxy)
- Interceptor adiciona `Authorization: Bearer <token>` em toda requisição autenticada
- Interceptor captura 401 → tenta refresh → se falhar, redireciona para login

### Serviços HTTP
```typescript
// Sempre tipados com generics, nunca `any`
getFluxoCaixa(ano: number): Observable<FluxoMensalDto[]> {
  return this.http.get<FluxoMensalDto[]>(`${this.api}/financeiro/fluxo?ano=${ano}`);
}
```

### Estado
- Signals para estado local de componente
- Services com BehaviorSubject para estado compartilhado entre rotas
- Evitar NgRx a menos que a complexidade justifique

### Rotas protegidas
```typescript
// Sempre usar AuthGuard nos módulos que exigem login
{ path: 'estudos', component: EstudosComponent, canActivate: [AuthGuard] }
```

### Módulos de Features (Standalone)
- Cada feature tem seu próprio `routes.ts`
- Lazy loading obrigatório: `loadChildren: () => import('./features/estudos/routes')`

## Nunca fazer no frontend
- NUNCA armazenar access token em localStorage (XSS risk)
- NUNCA fazer chamadas HTTP fora de Services
- NUNCA usar `any` no TypeScript
- NUNCA colocar lógica de negócio em componentes (vai para Services)
