# CareerHub — Project Memory

## O que é este projeto
Aplicação full-stack de gestão pessoal de carreira com módulos de:
- **Estudos**: tarefas por dia da semana com cálculo de horas
- **Posts**: publicações pessoais
- **Finanças**: despesas, recebíveis projetados e fluxo de caixa mensal
- **Conteúdo**: agregação de artigos .NET e Angular (sem persistência, apenas repasse ao frontend)

**Stack**: .NET 8 (C#) + Angular 18 | Sql Server | JWT via ASP.NET Identity | Clean Architecture

---

## Arquitetura Geral

```
CareerHub/
├── backend/   → .NET 8 Web API (Clean Architecture + DDD)
├── frontend/  → Angular 18 SPA
└── docs/      → ADRs e documentação
```

---

## Regras Globais (valem para TUDO)

### Nomenclatura
- **Português** para nomes de domínio (Estudo, Tarefa, Despesa, Recebivel, Post)
- **Inglês** para código técnico (controllers, services, repositories, DTOs)
- Sufixos obrigatórios: `Service`, `Repository`, `Controller`, `Handler`, `Validator`

### Git
- Commits sempre em inglês com prefixo: `feat:`, `fix:`, `refactor:`, `chore:`, `docs:`
- Branch pattern: `feature/nome-da-feature`, `fix/descricao-do-bug`

### Nunca fazer
- NUNCA expor entidades de domínio diretamente nas APIs — sempre usar DTOs/ViewModels
- NUNCA colocar lógica de negócio em Controllers
- NUNCA persistir conteúdo externo (.NET/Angular news) — apenas buscar e repassar
- NUNCA retornar stack traces em produção

### Sempre fazer
- SEMPRE usar Result Pattern (sem exceptions para fluxo de negócio)
- SEMPRE validar com FluentValidation antes de chegar na Application layer
- SEMPRE versionar endpoints: `/api/v1/...`
- SEMPRE incluir testes unitários para casos de uso (Application layer)
