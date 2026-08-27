# Documentacao de Arquitetura - Products Catalog

## Visao Geral

O projeto segue **Clean Architecture** com **CQRS** (Command Query
Responsibility Segregation) implementado via **MediatR**. A regra de
dependencia e sempre a mesma: as camadas de fora dependem das de dentro, e
nunca o contrario.

```
Api  →  Infrastructure  →  Application  →  Domain
```

(Infrastructure implementa portas definidas na Application; a Api e o
composition root que conhece todo mundo e monta o grafo de DI.)

## Domain (`src/ProductsCatalog.Domain`)

Sem dependencia de framework (a unica excecao consciente e o pacote MediatR,
usado so pelos marker interfaces `INotification`/`IRequest` para expressar
Domain Events - nenhum `IMediator` e usado aqui).

- **Entities/**: `Book`, `Customer`, `Order`, `OrderItem`, `Stock`, `User`,
  alem das bases `Product` e (em `Common/`) `BaseEntity`. Toda mutacao de
  estado passa por um metodo com regra de negocio (`Book.Update`,
  `Stock.Reserve`, `Order.Cancel`, etc.) - nunca um setter publico.
  `Customer` e `User` sao conceitos separados de proposito: `User` e uma
  conta com login (email/senha/role - ver secao "Autenticacao e autorizacao"
  abaixo), `Customer` e so quem recebe/paga um pedido, sem conta/login
  proprio - so precisa existir como cadastro.
- **ValueObjects/**: `Email` (imutavel, valida formato).
- **Events/**: `BaseDomainEvent`, `OrderCreatedEvent`, `OrderCanceledEvent`.
  Entidades acumulam eventos (`AddDomainEvent`) que sao publicados pela
  Infrastructure logo apos o `SaveChanges` ter sucesso (ver mais abaixo).
- **Enums/**: `EBookGenre`, `EOrderStatus`, `ERole` (`Administrator`/`Seller` - ver abaixo).
- **Exceptions/**: `DomainException` - violacao de regra de negocio.
- **Specifications/**: `ISpecification<T>`/`BaseSpecification<T>` (padrao
  Specification) + uma especificacao concreta por agregado
  (`BooksFilterSpecification`, etc.) para expressar filtro/ordenacao/paginacao
  sem o Domain conhecer EF Core.
- **Repositories/**: interfaces (`IRepository<T>` generico + uma por
  agregado: `IBookRepository`, `ICustomerRepository`, `IOrderRepository`,
  `IStockRepository`, `IUserRepository`).

## Application (`src/ProductsCatalog.Application`)

Aqui vive o CQRS propriamente dito.

- **Features/{Books,Customers,Orders,Stocks,Users}/**: cada feature tem `Commands/` e
  `Queries/`, e cada Command/Query tem sua propria pasta com
  `XCommand.cs` (o `record`), `XCommandValidator.cs` (FluentValidation,
  quando aplicavel) e `XCommandHandler.cs`. Esse e o "vertical slice":
  tudo que uma operacao precisa fica junto, em vez de espalhado por
  camadas de "todos os validators", "todos os handlers".
- **Common/Messaging/**: `ICommand<T>` e `IQuery<T>` - marcam a intencao
  (escrita vs leitura) e permitem que o `UnitOfWorkBehavior` saiba quando
  chamar `SaveChangesAsync` (so para Commands).
- **Common/Behaviors/**: pipeline do MediatR, executado nesta ordem para
  toda Command/Query:
  1. `UnhandledExceptionBehavior` - loga qualquer excecao nao tratada
  2. `ValidationBehavior` - roda os `IValidator<T>` (FluentValidation)
  3. `LoggingBehavior` - loga nome da request e tempo de execucao
  4. `UnitOfWorkBehavior` - se for um Command, chama `IUnitOfWork.SaveChangesAsync()`
- **Common/Interfaces/**: portas (`IUnitOfWork`, `IDateTime`, `IPasswordHasher`)
  implementadas na Infrastructure.
- **Common/Mappings/**: `IMapFrom<T>` + `MappingProfile` - qualquer DTO que
  implemente `IMapFrom<Entidade>` ganha um mapeamento AutoMapper automatico,
  sem precisar editar um profile central toda vez que um DTO novo aparece.
- **Common/Models/**: `PagedResult<T>` - retorno padrao das Queries de listagem.

### Por que Unit of Work em vez de cada repositorio salvar sozinho?

Varios casos de uso tocam mais de um agregado na mesma operacao (ex:
`CreateOrderCommandHandler` mexe em `Order` e em `Stock`). Se cada
`IRepository.AddAsync/Update` desse commit sozinho, uma falha no meio do
handler deixaria dado inconsistente (estoque reservado sem pedido criado).
Por isso os repositorios so marcam mudanca (`Add`/`Update`/`Remove`) e quem
persiste de fato e o `UnitOfWorkBehavior`, uma unica vez, so se o handler
inteiro terminar sem excecao.

## Infrastructure (`src/ProductsCatalog.Infrastructure`)

- **Persistence/ApplicationDbContext.cs**: implementa `IUnitOfWork` (a
  assinatura de `DbContext.SaveChangesAsync` ja bate com a interface) e,
  ao salvar com sucesso, publica os Domain Events acumulados nas entidades
  via `IPublisher.Publish` (MediatR).
- **Persistence/Configurations/**: um `IEntityTypeConfiguration<T>` por
  entidade (em vez de um `OnModelCreating` gigante).
- **Persistence/Repositories/**: `RepositoryBase<T>` (implementacao generica
  usando `SpecificationEvaluator` para traduzir `ISpecification<T>` em LINQ)
  + uma classe concreta por agregado.
- **Messaging/** e **Caching/**: pastas de extensao (ver README de cada uma)
  para quando o projeto precisar de mensageria assincrona (ex: Kafka) ou
  cache - nao implementadas ainda, deixadas como ponto de extensao explicito.
- **Services/**: implementacoes das portas da Application (`DateTimeService`,
  `PasswordHasher` - PBKDF2 sem dependencia externa).

## Api (`src/ProductsCatalog.Api`)

- **Controllers/**: finos - so traduzem HTTP em `sender.Send(command/query)`.
  Nenhuma logica de negocio aqui.
- **Middleware/ExceptionHandlingMiddleware.cs**: converte
  `ValidationException` → 400, `NotFoundException` → 404, `DomainException`
  → 400, qualquer outra coisa → 500. Os controllers nao tem try/catch.
- **Program.cs**: composition root - so chama `AddApplication()` e
  `AddInfrastructure()` e configura o pipeline HTTP.

## Fluxo completo: criar um pedido

```
1. POST /api/orders  →  OrdersController.CreateAsync
2. sender.Send(CreateOrderCommand)
3. Pipeline: UnhandledException → Validation → Logging → UnitOfWork → Handler
4. CreateOrderCommandHandler:
     valida que o Customer existe (senao, NotFoundException -> 404)
     para cada item: busca o Book (preco), busca o Stock, chama Stock.Reserve()
     cria o Order (que valida invariantes e dispara OrderCreatedEvent)
5. UnitOfWorkBehavior chama SaveChangesAsync (unica escrita no banco)
6. ApplicationDbContext publica OrderCreatedEvent via MediatR
7. Controller retorna 201 Created com o Id do pedido
```

Se o estoque for insuficiente, `Stock.Reserve` lanca `DomainException` antes
do passo 5 - nada e persistido.

## Autenticacao e autorizacao

- **Emissao do token**: `POST /api/auth/login` (`LoginCommandHandler`) verifica
  a senha (`IPasswordHasher.Verify`, PBKDF2 - mesma implementacao usada no
  cadastro) e, se valida, pede um JWT a `IJwtTokenGenerator`. A implementacao
  (`Infrastructure/Services/JwtTokenGenerator`) assina o token na mao com
  `HMACSHA256`, no mesmo espirito do `PasswordHasher`: nenhum pacote novo
  precisou ser adicionado so para emitir o token (a validacao, do lado da Api,
  usa o pacote `Microsoft.AspNetCore.Authentication.JwtBearer`, que ja estava
  referenciado desde o inicio do projeto mas nunca configurado).
- **Claims**: o payload usa os nomes longos de `ClaimTypes.NameIdentifier` /
  `ClaimTypes.Email` / `ClaimTypes.Role` - de proposito, porque
  `TokenValidationParameters.DefaultRoleClaimType` do ASP.NET Core ja e
  `ClaimTypes.Role`. Isso permite `[Authorize(Roles = nameof(ERole.Administrator))]`
  funcionar sem nenhuma configuracao adicional de mapeamento de claims.
- **Autorizacao por role**: a policy padrao (`FallbackPolicy`, em `Program.cs`)
  exige apenas um usuario autenticado - e o suficiente para Seller criar/ver
  pedidos. Endpoints exclusivos de Administrator (CRUD de livros, estoque,
  clientes, usuarios, cancelamento de pedido) tem
  `[Authorize(Roles = nameof(ERole.Administrator))]` **na action**, nunca no
  controller inteiro nos controllers com permissoes mistas
  (`BooksController`, `StocksController`, `CustomersController`): varios
  `[Authorize]` no mesmo endpoint (um no controller, um no metodo) combinam
  com AND, nao OR - um controller-level `Roles=Administrator` "vazaria" para
  as actions de leitura, que devem ficar abertas a qualquer usuario
  autenticado (inclusive Seller, que precisa ler livros/estoque/clientes para
  montar um pedido). `UsersController` e o unico caso onde o controller
  inteiro e Administrator-only, porque nenhuma action ali faz sentido pra
  Seller.
- **Bootstrap**: como e preciso estar logado para cadastrar usuarios,
  `ApplicationDbContextSeeder` cria um Administrator padrao
  (`admin@email.com`/`admin`) logo depois do `EnsureCreated()`, so em
  Development - mesmo bloco que ja criava o schema (ver nota sobre
  migrations). E idempotente (checa por email antes de inserir) e tolerante
  a corrida entre instancias concorrentes subindo ao mesmo tempo (mesmo
  padrao do `SchemaCreationLock` em `ApiWebApplicationFactory` - ver Testes).

## Testes

- **Domain.Tests**: testa as regras nas entidades diretamente (sem mocks).
- **Application.Tests**: testa os handlers mockando as interfaces de
  repositorio (Moq) - nao sobe banco nem HTTP.
- **Api.IntegrationTests**: sobe a Api inteira via `WebApplicationFactory`
  e bate nos endpoints reais. Por padrao usa banco InMemory (um por classe
  de teste); se a variavel `ConnectionStrings__DefaultConnection` estiver
  definida (como no `docker-compose.yml`), roda contra um Postgres real.

## O que ficou fora do escopo (de proposito)

- **Excluir um Customer com pedidos existentes**: `Order.CustomerId` tem uma
  FK real para `Customers` (`OnDelete(Restrict)`), entao o banco rejeita a
  exclusao - mas o handler (`DeleteCustomerCommandHandler`) nao verifica isso
  antes, entao o erro que chega pro usuario e uma excecao de banco crua (500),
  nao uma mensagem de negocio amigavel. Mesma lacuna que ja existia antes
  para `User`/`Order` (nunca foi tratada) - so mudou de entidade.
- **Revogacao de token/refresh token**: o JWT emitido no login e valido por
  8h e nao pode ser invalidado no servidor antes de expirar (ex: ao trocar de
  senha, ou um "logout em todos os dispositivos") - implementar isso exigiria
  guardar estado (uma blacklist de `jti`, tipicamente em cache/Redis, checada a
  cada request). `LogoutCommand`/`Caching/` documentam isso como extension
  point. Tambem nao ha refresh token nesta versao: expirado o token, e preciso
  logar de novo.
- Migrations do EF Core regeneradas (o ambiente onde este refactor foi feito
  nao tinha o `dotnet` CLI disponivel para rodar `dotnet ef migrations add`).
- Mensageria (Kafka) e cache (Redis) - pastas `Messaging/` e `Caching/`
  deixadas como extension point, nao implementadas.
