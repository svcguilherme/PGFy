# CareerHub — Architecture Decision Records

## ADR-001: Clean Architecture com Bounded Contexts modulares

**Decisão**: Monolito modular com Clean Architecture e bounded contexts claros por domínio.

**Motivo**: Projeto pessoal em evolução — monolito modular dá a clareza do DDD sem overhead de microserviços. Fácil de extrair em serviços separados no futuro se necessário.

**Bounded Contexts**:
- `Identity` — usuários, autenticação, autorização
- `Estudos` — agenda de estudos por dia da semana
- `Posts` — publicações pessoais
- `Financeiro` — despesas, recebíveis, fluxo de caixa
- `Conteudo` — agregação de artigos externos (sem persistência)

---

## ADR-002: RSS Feeds para conteúdo externo (sem persistência)

**Decisão**: Buscar conteúdo .NET/Angular via RSS com cache em memória (1h TTL).

**Motivo**: Evitar custos de storage, problemas de copyright e manutenção de scraping. RSS é a forma oficial que os blogs oferecem para consumo de conteúdo.

**Fontes**: .NET Blog, ASP.NET Blog, Angular Blog, DEV.to (tags: dotnet, angular)

---

## ADR-003: JWT com Refresh Token

**Decisão**: Access Token (15min) + Refresh Token (7 dias, armazenado no banco).

**Motivo**: Balanceia segurança e UX — access token curto minimiza risco de vazamento, refresh token longo evita logins frequentes.

---

## ADR-004: SQL Server como banco principal

**Decisão**: SQL Server via EF Core 8 com Microsoft.EntityFrameworkCore.SqlServer.

**Motivo**: Stack Microsoft alinhado com .NET 8 / ASP.NET Core Identity. Melhor integração nativa com o ecossistema e ferramentas como SSMS e Azure SQL. Docker via imagem oficial `mcr.microsoft.com/mssql/server:2022-latest`.
