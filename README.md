# Products Catalog

Sistema de catalogo de produtos em .NET 8, estruturado como referencia de
**Clean Architecture** + **CQRS com MediatR**, com um frontend em **React**
e um **docker-compose** para rodar a suite de testes. Gerencia um catalogo
de livros, criacao de pedidos e controle de estoque.

## Estrutura da solution

```
ProductsCatalog.sln
├── src/
│   ├── ProductsCatalog.Domain/          → nucleo, sem dependencia de framework
│   ├── ProductsCatalog.Application/     → CQRS (Commands/Queries + MediatR), depende so de Domain
│   ├── ProductsCatalog.Infrastructure/  → EF Core, repositorios; depende de Application
│   └── ProductsCatalog.Api/             → composition root (controllers, Program.cs)
├── tests/
│   ├── ProductsCatalog.Domain.Tests/         → regras de negocio das entidades
│   ├── ProductsCatalog.Application.Tests/    → handlers (Moq para os repositorios)
│   └── ProductsCatalog.Api.IntegrationTests/ → API real de ponta a ponta (WebApplicationFactory)
├── client/            → frontend React + TypeScript (ver client/README.md)
├── docker/            → Dockerfile usado so para rodar os testes em container
└── docker-compose.yml → sobe Postgres + roda toda a suite de testes
```

Veja `ARCHITECTURE.md` para o detalhamento de cada camada e das decisoes de
design (CQRS, Specification pattern, Unit of Work, Domain Events).

## Tecnologias

- **.NET 8**, **Entity Framework Core** (InMemory / PostgreSQL / SQL Server)
- **MediatR** (CQRS: Commands/Queries + pipeline behaviors)
- **FluentValidation** (validacao dos Commands/Queries)
- **AutoMapper** (mapeamento Entidade ↔ DTO)
- **React + TypeScript + Vite**, React Router, TanStack Query, Axios
- **xUnit + Moq + FluentAssertions** para os testes
- **Docker Compose** para rodar os testes em container

## Como rodar a Api

Requer .NET 8 SDK.

```bash
cd src/ProductsCatalog.Api
dotnet run
```

Sem `ConnectionStrings:DefaultConnection` configurada, a Api usa banco
InMemory automaticamente. O Swagger fica em `/swagger`.

## Como rodar o frontend

Requer Node 20+.

```bash
cd client
npm install
npm run dev
```

Veja `client/README.md` para a organizacao do projeto React.

## Como rodar os testes

**Localmente** (com .NET 8 SDK instalado):

```bash
dotnet test ProductsCatalog.sln
```

**Via Docker** (nao precisa ter o .NET instalado - sobe um Postgres real e
roda toda a suite dentro de um container):

```bash
docker compose up --build --abort-on-container-exit
```

## Notas importantes desta refatoracao

- **Autenticacao/JWT**: o pacote `Microsoft.AspNetCore.Authentication.JwtBearer`
  esta referenciado e `app.UseAuthentication()/UseAuthorization()` sao
  chamados, mas nenhum esquema de autenticacao e configurado (isso ja vinha
  assim no projeto original). O endpoint `GET /api/orders/my-orders` depende
  de um claim de usuario autenticado que hoje nao existe - login/JWT ficam
  fora do escopo desta refatoracao (focada em Clean Architecture + CQRS).
- **Migrations do EF Core**: o modelo mudou o suficiente (Configurations
  separadas, `Amount` calculado, `PasswordHash`, etc.) que a migration antiga
  foi descartada em vez de adaptada. Veja
  `src/ProductsCatalog.Infrastructure/Persistence/Migrations/README.md` para
  gerar a primeira migration com `dotnet ef`.
