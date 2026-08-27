# Products Catalog

Sistema de catalogo de produtos em .NET 8, estruturado como referencia de
**Clean Architecture** + **CQRS com MediatR**, com um frontend em **React**
e um **docker-compose** para subir o sistema localmente (Postgres + Api +
frontend) e para rodar a suite de testes. Gerencia um catalogo
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
└── docker-compose.yml → sobe Postgres + Api + frontend (uso local) e,
                        via perfil "test", a suite de testes
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
- **Docker Compose** para uso local (backend + frontend) e para rodar os testes em container

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

## Como rodar tudo com Docker (backend + frontend)

Nao precisa ter .NET nem Node instalados - o docker-compose builda e sobe
tudo (Postgres real, Api e frontend):

```bash
docker compose up --build
```

- Api / Swagger: http://localhost:5145/swagger
- Frontend:      http://localhost:5173

Nao ha migrations do EF Core geradas ainda (ver nota abaixo), entao a Api
cria o schema do banco automaticamente ao subir em ambiente Development -
nao precisa rodar nada manualmente antes. Os dados do Postgres ficam num
volume Docker (`postgres_data`) e sobrevivem a reinicios (`docker compose
down -v` para zerar).

O frontend roda com hot-reload (o codigo em `client/` e montado dentro do
container), entao editar arquivos localmente reflete no navegador sem
precisar rebuildar a imagem.

### Se uma mudanca no backend nao aparecer

O `client` tem hot-reload, mas a `api` e um binario compilado numa imagem -
se voce editou codigo backend e `docker compose up --build` nao pegou a
mudanca (o Docker as vezes reaproveita uma camada em cache), rode o script
abaixo (PowerShell) pra reconstruir do zero e ja subir de novo:

```powershell
./scripts/rebuild.ps1                # reconstroi tudo (Postgres + Api + frontend)
./scripts/rebuild.ps1 -Service api   # so a Api, mais rapido
```

Ele faz `docker compose down -v` (zera o volume do Postgres tambem) +
`docker compose build --no-cache` + `docker compose up`.

## Autenticacao e permissoes

A Api exige login (JWT) em praticamente todos os endpoints. Ha duas roles:

| Role            | Pode fazer                                                              |
|------------------|---------------------------------------------------------------------------|
| **Administrator** | Tudo: CRUD de livros, estoque, clientes, usuarios, e criar/ver/cancelar pedidos. |
| **Seller**         | Só criar e ver pedidos (`POST/GET /api/orders`). Sem acesso a livros/estoque/clientes/usuarios nem a cancelar pedidos. |

No frontend, o menu (Books/Stocks/Customers/Users) so aparece para quem
logou como Administrator - um Seller ve so "Orders". Isso e so uma
conveniencia de UI: a Api tambem recusa (403) qualquer tentativa de acessar
esses endpoints com um token de Seller, entao a restricao real esta no
backend.

### Usuario padrao (bootstrap)

Como e preciso estar logado para cadastrar usuarios, a Api cria automaticamente
um usuario Administrator na primeira vez que sobe em ambiente Development:

- **email:** `admin@email.com`
- **senha:** `admin`

**Troque essa senha assim que possivel** (nao ha endpoint de troca de senha
nesta versao - veja a nota em `UserForm`/`UpdateUserCommand`: seria um fluxo
a parte). Use esse login so para criar os usuarios "de verdade" depois.

### Login/logout

- `POST /api/auth/login` com `{ "email": "...", "password": "..." }` retorna
  um JWT (valido por 8h - sem refresh token nesta versao, e preciso logar de
  novo apos expirar) junto com os dados do usuario logado (id/email/role).
- `POST /api/auth/logout` existe para completar o fluxo, mas JWT e stateless:
  nao ha revogacao de token no servidor, o "logout" so descarta o token
  guardado no navegador. Ver comentario em `LogoutCommand` para detalhes e o
  porque disso ser aceitavel nesta versao.
- O segredo usado para assinar/validar os tokens (`Jwt:Secret`) so tem um
  valor em `appsettings.Development.json`, e e explicitamente um valor de
  **desenvolvimento** (nao usar em producao). Fora de `Development`, a Api
  falha ao subir se essa configuracao nao existir - de proposito, para nao
  aceitar tokens assinados com um segredo fraco/padrao silenciosamente.

## Como rodar os testes

**Localmente** (com .NET 8 SDK instalado):

```bash
dotnet test ProductsCatalog.sln
```

**Via Docker** (nao precisa ter o .NET instalado - sobe um Postgres
descartavel, separado do banco usado acima para uso local, e roda toda a
suite dentro de um container):

```bash
docker compose --profile test up --build --abort-on-container-exit postgres-tests tests
```

(os nomes dos servicos no final do comando importam: sem eles, `--profile
test` so adicionaria `postgres-tests`/`tests` aos servicos padrao - postgres/api/client
tambem subiriam junto.)

## Notas importantes desta refatoracao

- **Autenticacao/JWT**: implementada (ver secao "Autenticacao e permissoes"
  acima). O endpoint `GET /api/orders/my-orders` continua nao funcional na
  pratica: ele usa o Id do `User` autenticado, mas pedidos sao vinculados a
  um `Customer` (entidade separada, sem login) - nao ha relacao entre as
  duas. Isso e uma limitacao preexistente e ficou fora do escopo desta
  mudanca (ver comentario no endpoint).
- **Migrations do EF Core**: o modelo mudou o suficiente (Configurations
  separadas, `Amount` calculado, `PasswordHash`, etc.) que a migration antiga
  foi descartada em vez de adaptada. Veja
  `src/ProductsCatalog.Infrastructure/Persistence/Migrations/README.md` para
  gerar a primeira migration com `dotnet ef`.
- **Customer e uma entidade separada de User**: `User` e uma conta com login
  (email/senha/role - sem autenticacao real ainda, ver acima); `Customer` e
  so quem recebe/paga um pedido, sem senha. `Order.CustomerId` agora tem uma
  FK real para `Customers` (antes apontava para `Users`) e todo pedido exige
  um `Customer` existente (`POST /api/orders` retorna 404 se o `customerId`
  nao existir). Gerencie clientes em `/customers` no frontend ou via
  `POST/GET/PUT/DELETE /api/customers` na Api.
- **Se voce ja rodou o projeto antes desta mudanca**: como nao ha migrations
  (ver acima), o schema e criado uma unica vez via `EnsureCreated()` - ele
  NAO aplica alteracoes incrementais a um banco que ja existe. Isso vale
  tambem para a introducao do login: a coluna `Role` de `Users` mudou de
  texto livre para um enum inteiro, e o usuario admin padrao so e criado na
  criacao inicial do schema. Se o seu Postgres local (volume
  `postgres_data`) ja existia antes desta mudanca (ou da mudanca anterior,
  que introduziu `Customers`), rode `docker compose down -v` para apagar o
  volume e `docker compose up --build` para recriar o schema do zero,
  ja com o usuario admin padrao.
