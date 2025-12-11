# Documentação de Arquitetura - Products Catalog

## Visão Geral

Este documento descreve a arquitetura do projeto **Products Catalog**, um sistema desenvolvido seguindo os princípios de **Clean Architecture** e **Domain Driven Design (DDD)**.

## Princípios Arquiteturais

### Clean Architecture

O projeto segue a Clean Architecture, que promove:

- **Independência de frameworks**: O código de negócio não depende de frameworks externos
- **Testabilidade**: Regras de negócio podem ser testadas sem UI, banco de dados ou qualquer elemento externo
- **Independência de UI**: A interface pode ser facilmente alterada sem afetar o sistema
- **Independência de banco de dados**: Pode-se trocar Oracle, SQL Server, PostgreSQL, MongoDB, etc.
- **Independência de qualquer agente externo**: As regras de negócio não sabem nada sobre o mundo externo

### Domain Driven Design (DDD)

O projeto aplica conceitos de DDD:

- **Entidades**: Objetos com identidade única (Book, Order, User, Stock)
- **Value Objects**: Objetos imutáveis sem identidade (Email)
- **Domain Services**: Lógica de domínio que não pertence a uma entidade específica
- **Repositories**: Abstração para persistência de entidades

## Estrutura de Camadas

### 1. API (Presentation Layer)

**Localização**: `src/API/`

**Responsabilidades**:
- Receber requisições HTTP
- Validar entrada básica
- Chamar Application Services
- Retornar respostas HTTP
- Configurar middleware (CORS, Autenticação, Swagger)

**Componentes Principais**:
- `Controllers/`: Controllers REST que expõem endpoints da API
  - `BooksController.cs`: Operações CRUD de livros
  - `OrdersController.cs`: Gerenciamento de pedidos
  - `StocksController.cs`: Controle de estoque
  - `UsersController.cs`: Gerenciamento de usuários
- `Program.cs`: Configuração e inicialização da aplicação

**Dependências**: Application Layer

### 2. Application (Application Layer)

**Localização**: `src/Application/`

**Responsabilidades**:
- Orquestrar casos de uso
- Coordenar transações
- Validar dados de entrada
- Mapear entre DTOs e entidades de domínio
- Implementar lógica de aplicação (não de negócio)

**Componentes Principais**:

#### Services (`Services/`)
Application Services que coordenam operações:
- `BooksAppService`: Gerencia operações de livros
- `OrdersAppService`: Gerencia operações de pedidos
- `StocksAppService`: Gerencia operações de estoque
- `UsersAppService`: Gerencia operações de usuários

#### DTOs (`DTOs/`)
Objetos de transferência de dados:
- `BookDto`, `OrderDto`, `StockDto`, `UserDto`
- `OrderItemDto`, `AddStockDto`
- `TextFilterDto`, `EmailDto`

#### UseCases (`UseCases/`)
Handlers CQRS usando MediatR:
- `CancelOrder/`: Handler para cancelamento de pedidos
- `CreateOrder/`: Handler para criação de pedidos

#### AutoMapper (`AutoMapper/`)
Configuração de mapeamento entre entidades e DTOs.

**Dependências**: Domain Layer

### 3. Domain (Domain Layer)

**Localização**: `src/Domain/`

**Responsabilidades**:
- Conter regras de negócio puras
- Definir entidades e value objects
- Definir interfaces de repositório
- Validar regras de domínio

**Componentes Principais**:

#### Entities (`Entities/`)
Entidades de domínio com comportamento:
- `Book`: Representa um livro no catálogo
  - Validações: título obrigatório (máx 30 chars), autor obrigatório (3-30 chars)
- `Order`: Representa um pedido
  - Gerencia itens do pedido
  - Controla status (Pending, Confirmed, Canceled)
- `OrderItem`: Item de um pedido
- `Stock`: Controle de estoque de livros
- `User`: Usuário do sistema

#### Value Objects (`ValueObject/`)
Objetos imutáveis:
- `Email`: Value object para email com validação

#### Enums (`Enums/`)
Enumerações de domínio:
- `EBookGenre`: Gêneros literários
- `EOrderStatus`: Status de pedidos

#### Interfaces (`Interfaces/`)
Contratos:
- `IRepository<T>`: Interface genérica para repositórios

**Dependências**: Nenhuma (camada mais interna)

### 4. Infrastructure (Infrastructure Layer)

**Localização**: `src/Infra/`

**Responsabilidades**:
- Implementar acesso a dados
- Configurar Entity Framework Core
- Implementar repositórios
- Gerenciar migrações

**Componentes Principais**:

#### ApplicationDbContext (`ApplicationDbContext.cs`)
Contexto do Entity Framework Core:
- Define DbSets para todas as entidades
- Configura mapeamentos e relacionamentos
- Define índices e constraints

#### Repository (`Repository.cs`)
Implementação genérica do padrão Repository:
- `CreateAsync`: Criar entidade
- `ReadAsync`: Ler por ID
- `UpdateAsync`: Atualizar entidade
- `DeleteAsync`: Deletar entidade
- `FindAsync`: Buscar com filtros e paginação

#### Migrations (`Migrations/`)
Migrações do banco de dados geradas pelo EF Core.

**Dependências**: Domain Layer

### 5. Shared

**Localização**: `src/Shared/`

**Responsabilidades**:
- Componentes compartilhados entre camadas
- Entidades base
- Exceções de domínio

**Componentes**:
- `Entity`: Classe base para todas as entidades (Id, CreatedAt, UpdateAt)
- `Product`: Classe base para produtos
- `DomainException`: Exceção customizada para erros de domínio

## Padrões de Design Implementados

### Repository Pattern

**Objetivo**: Abstrair o acesso a dados

**Implementação**:
- Interface genérica `IRepository<T>` no Domain
- Implementação `Repository<T>` no Infra
- Permite trocar implementação de persistência sem afetar outras camadas

**Benefícios**:
- Testabilidade (mock de repositórios)
- Flexibilidade (múltiplos bancos de dados)
- Separação de responsabilidades

### CQRS (Command Query Responsibility Segregation)

**Objetivo**: Separar operações de leitura e escrita

**Implementação**:
- Uso do MediatR para handlers
- Commands para operações que modificam estado
- Queries para operações de leitura

**Exemplo**:
- `CancelOrderHandler`: Command que cancela um pedido e atualiza estoque

### Dependency Injection

**Objetivo**: Inversão de controle e baixo acoplamento

**Implementação**:
- Container nativo do ASP.NET Core
- Registro de serviços em `DependencyInjection.cs` de cada camada
- Injeção via construtores

### DTOs (Data Transfer Objects)

**Objetivo**: Transferir dados entre camadas sem expor entidades

**Implementação**:
- DTOs específicos para cada operação
- Mapeamento via AutoMapper
- Validação na camada de aplicação

## Fluxo de Dados

### Exemplo: Criar um Livro

```
1. Cliente → HTTP POST /api/Books/Create
2. BooksController → Recebe BookDto
3. BooksController → Chama BooksAppService.CreateAsync()
4. BooksAppService → Mapeia BookDto para Book (entidade)
5. BooksAppService → Chama IRepository<Book>.CreateAsync()
6. Repository → Persiste no banco via DbContext
7. Repository → Retorna
8. BooksAppService → Retorna Guid (ID do livro)
9. BooksController → Retorna HTTP 200 com ID
```

### Exemplo: Cancelar Pedido (CQRS)

```
1. Cliente → HTTP POST /api/Orders/{id}/cancel
2. OrdersController → Chama MediatR com CancelOrderRequest
3. CancelOrderHandler → Lê Order do repositório
4. CancelOrderHandler → Valida regras de negócio
5. CancelOrderHandler → Atualiza estoque para cada item
6. CancelOrderHandler → Atualiza status do pedido
7. CancelOrderHandler → Retorna CancelOrderResponse
8. OrdersController → Retorna HTTP 200
```

## Banco de Dados

### Suporte Multi-Banco

O projeto suporta três opções de banco de dados:

1. **InMemory Database** (padrão para desenvolvimento)
2. **PostgreSQL** (produção recomendada)
3. **SQL Server**

A escolha é feita automaticamente baseada na connection string configurada.

### Modelo de Dados

**Tabelas Principais**:
- `Books`: Catálogo de livros
- `Users`: Usuários do sistema
- `Orders`: Pedidos
- `OrderItems`: Itens de pedidos
- `Stocks`: Controle de estoque

**Relacionamentos**:
- `Order` → `User` (Many-to-One)
- `Order` → `OrderItem` (One-to-Many)
- `OrderItem` → `Book` (Many-to-One)
- `Stock` → `Book` (One-to-One)

## Segurança

- **Autenticação JWT**: Tokens para acesso aos endpoints
- **Autorização por Roles**: Admin e Client
- **Validação de Entrada**: Validação de DTOs na camada de aplicação
- **Validação de Domínio**: Regras de negócio nas entidades

## Testes

### Estrutura de Testes

**Localização**: `src/Tests/`

**Cobertura**:
- Application Services
- Regras de negócio
- Validações de domínio

**Frameworks**:
- xUnit (assumido pela estrutura)

## Melhores Práticas Aplicadas

1. **Separação de Responsabilidades**: Cada camada tem responsabilidade bem definida
2. **Dependency Inversion**: Dependências apontam para dentro (Domain)
3. **Single Responsibility**: Cada classe tem uma única responsabilidade
4. **DRY (Don't Repeat Yourself)**: Reutilização através de classes base e interfaces genéricas
5. **SOLID Principles**: Aplicação dos princípios SOLID em toda a arquitetura

## Considerações de Escalabilidade

- **Repository Pattern**: Facilita adicionar cache ou outras otimizações
- **CQRS**: Permite otimizar leituras e escritas separadamente
- **Dependency Injection**: Facilita adicionar novos serviços
- **Clean Architecture**: Permite evoluir a aplicação sem grandes refatorações

## Próximos Passos Sugeridos

1. Adicionar cache para consultas frequentes
2. Implementar eventos de domínio para integração assíncrona
3. Adicionar logging estruturado
4. Implementar health checks
5. Adicionar métricas e observabilidade
6. Implementar testes de integração
7. Adicionar documentação de API mais detalhada

