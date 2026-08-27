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

- **Entities/**: `Book`, `Order`, `OrderItem`, `Stock`, `User`, alem das
  bases `Product` e (em `Common/`) `BaseEntity`. Toda mutacao de estado passa
  por um metodo com regra de negocio (`Book.Update`, `Stock.Reserve`,
  `Order.Cancel`, etc.) - nunca um setter publico.
- **ValueObjects/**: `Email` (imutavel, valida formato).
- **Events/**: `BaseDomainEvent`, `OrderCreatedEvent`, `OrderCanceledEvent`.
  Entidades acumulam eventos (`AddDomainEvent`) que sao publicados pela
  Infrastructure logo apos o `SaveChanges` ter sucesso (ver mais abaixo).
- **Enums/**: `EBookGenre`, `EOrderStatus`.
- **Exceptions/**: `DomainException` - violacao de regra de negocio.
- **Specifications/**: `ISpecification<T>`/`BaseSpecification<T>` (padrao
  Specification) + uma especificacao concreta por agregado
  (`BooksFilterSpecification`, etc.) para expressar filtro/ordenacao/paginacao
  sem o Domain conhecer EF Core.
- **Repositories/**: interfaces (`IRepository<T>` generico + uma por
  agregado: `IBookRepository`, `IOrderRepository`, `IStockRepository`,
  `IUserRepository`).

## Application (`src/ProductsCatalog.Application`)

Aqui vive o CQRS propriamente dito.

- **Features/{Books,Orders,Stocks,Users}/**: cada feature tem `Commands/` e
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
     para cada item: busca o Book (preco), busca o Stock, chama Stock.Reserve()
     cria o Order (que valida invariantes e dispara OrderCreatedEvent)
5. UnitOfWorkBehavior chama SaveChangesAsync (unica escrita no banco)
6. ApplicationDbContext publica OrderCreatedEvent via MediatR
7. Controller retorna 201 Created com o Id do pedido
```

Se o estoque for insuficiente, `Stock.Reserve` lanca `DomainException` antes
do passo 5 - nada e persistido.

## Testes

- **Domain.Tests**: testa as regras nas entidades diretamente (sem mocks).
- **Application.Tests**: testa os handlers mockando as interfaces de
  repositorio (Moq) - nao sobe banco nem HTTP.
- **Api.IntegrationTests**: sobe a Api inteira via `WebApplicationFactory`
  e bate nos endpoints reais. Por padrao usa banco InMemory (um por classe
  de teste); se a variavel `ConnectionStrings__DefaultConnection` estiver
  definida (como no `docker-compose.yml`), roda contra um Postgres real.

## O que ficou fora do escopo (de proposito)

- Autenticacao/JWT real (login, emissao de token) - ver nota no `README.md`.
- Migrations do EF Core regeneradas (o ambiente onde este refactor foi feito
  nao tinha o `dotnet` CLI disponivel para rodar `dotnet ef migrations add`).
- Mensageria (Kafka) e cache (Redis) - pastas `Messaging/` e `Caching/`
  deixadas como extension point, nao implementadas.
