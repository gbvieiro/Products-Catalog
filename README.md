# Products Catalog

Sistema de catálogo de produtos desenvolvido em .NET 8 seguindo os princípios de **Clean Architecture** e **Domain Driven Design (DDD)**. O projeto gerencia um catálogo de livros, criação de pedidos e controle de estoque.

## 📋 Índice

- [Arquitetura](#arquitetura)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Tecnologias](#tecnologias)
- [Pré-requisitos](#pré-requisitos)
- [Como Executar](#como-executar)
- [Testes](#testes)

## 🏗️ Arquitetura

O projeto segue os princípios de **Clean Architecture**, organizando o código em camadas bem definidas com responsabilidades separadas:

```
┌─────────────────────────────────────────┐
│           API (Presentation)            │
│         Controllers REST                 │
└─────────────────┬───────────────────────┘
                  │
┌─────────────────▼───────────────────────┐
│        Application (Application)         │
│   Services, DTOs, UseCases (CQRS)        │
└─────────────────┬───────────────────────┘
                  │
┌─────────────────▼───────────────────────┐
│          Domain (Domain)                 │
│   Entities, Value Objects, Interfaces    │
└─────────────────┬───────────────────────┘
                  │
┌─────────────────▼───────────────────────┐
│       Infrastructure (Infra)            │
│   DbContext, Repository, Migrations     │
└─────────────────────────────────────────┘
```

### Camadas

#### 1. **API (Presentation Layer)**
- **Responsabilidade**: Interface HTTP/REST, recebe requisições e retorna respostas
- **Componentes**: Controllers, configuração do Swagger, middleware de autenticação
- **Dependências**: Application Layer

#### 2. **Application (Application Layer)**
- **Responsabilidade**: Orquestração de casos de uso, validação de entrada, mapeamento de DTOs
- **Componentes**:
  - **Services**: Application Services que coordenam operações de negócio
  - **DTOs**: Objetos de transferência de dados
  - **UseCases**: Handlers CQRS usando MediatR para operações complexas
  - **AutoMapper**: Mapeamento entre entidades e DTOs
- **Dependências**: Domain Layer

#### 3. **Domain (Domain Layer)**
- **Responsabilidade**: Regras de negócio, entidades, value objects
- **Componentes**:
  - **Entities**: Entidades de domínio (Book, Order, Stock, User)
  - **Value Objects**: Objetos de valor (Email)
  - **Enums**: Enumerações de domínio (EBookGenre, EOrderStatus)
  - **Interfaces**: Contratos de repositório
- **Dependências**: Nenhuma (camada mais interna)

#### 4. **Infrastructure (Infrastructure Layer)**
- **Responsabilidade**: Acesso a dados, implementação de repositórios, configuração do banco
- **Componentes**:
  - **ApplicationDbContext**: Contexto do Entity Framework Core
  - **Repository**: Implementação do padrão Repository
  - **Migrations**: Migrações do banco de dados
- **Dependências**: Domain Layer

#### 5. **Shared**
- **Responsabilidade**: Componentes compartilhados entre camadas
- **Componentes**: Entidades base, exceções de domínio

### Padrões de Design

#### Repository Pattern
Abstração do acesso a dados através da interface `IRepository<T>`, permitindo:
- Testabilidade facilitada (mock de repositórios)
- Flexibilidade para trocar implementações de persistência
- Separação de responsabilidades

#### CQRS (Command Query Responsibility Segregation)
Uso do MediatR para separar comandos (writes) de consultas (reads):
- **Commands**: Operações que modificam estado (ex: CancelOrder)
- **Queries**: Operações de leitura (ex: GetBooks)

#### Dependency Injection
Inversão de controle através do container nativo do ASP.NET Core:
- Registro de serviços em `DependencyInjection.cs` de cada camada
- Injeção via construtores

#### DTOs (Data Transfer Objects)
Objetos leves para transferência de dados entre camadas, evitando expor entidades de domínio diretamente.

## 📁 Estrutura do Projeto

```
Products-Catalog/
├── src/
│   ├── API/                    # Camada de Apresentação
│   │   ├── Controllers/        # Controllers REST
│   │   └── Program.cs          # Configuração da aplicação
│   │
│   ├── Application/            # Camada de Aplicação
│   │   ├── Services/          # Application Services
│   │   ├── DTOs/              # Data Transfer Objects
│   │   ├── UseCases/          # Handlers CQRS (MediatR)
│   │   └── AutoMapper/        # Configuração de mapeamento
│   │
│   ├── Domain/                # Camada de Domínio
│   │   ├── Entities/          # Entidades de domínio
│   │   ├── ValueObject/       # Value Objects
│   │   ├── Enums/             # Enumerações
│   │   └── Interfaces/        # Contratos
│   │
│   ├── Infra/                 # Camada de Infraestrutura
│   │   ├── ApplicationDbContext.cs
│   │   ├── Repository.cs      # Implementação do Repository
│   │   └── Migrations/        # Migrações EF Core
│   │
│   ├── Shared/                # Componentes Compartilhados
│   │   ├── Entities/          # Entidades base
│   │   └── Exceptions/        # Exceções de domínio
│   │
│   └── Tests/                 # Testes Unitários
│
└── README.md
```

## 🛠️ Tecnologias

- **.NET 8**: Framework principal
- **Entity Framework Core**: ORM para acesso a dados
- **PostgreSQL/SQL Server/InMemory**: Suporte a múltiplos bancos de dados
- **MediatR**: Implementação de CQRS
- **AutoMapper**: Mapeamento objeto-objeto
- **Swagger/OpenAPI**: Documentação da API
- **xUnit**: Framework de testes (assumido pela estrutura)

## 📦 Pré-requisitos

- Visual Studio 2022 ou superior
- .NET 8 SDK
- (Opcional) PostgreSQL ou SQL Server, se não usar banco em memória

## 🚀 Como Executar

1. Abra o arquivo `.sln` em `src\product-catalog.sln` com Visual Studio 2022
2. Configure o projeto `API` como projeto de inicialização
3. Execute o projeto usando `F5`
4. Acesse o Swagger através da URL exibida no navegador (geralmente `https://localhost:XXXX/swagger`)

### Configuração do Banco de Dados

O projeto suporta três modos de operação:

1. **InMemory Database** (padrão): Se não houver connection string configurada
2. **PostgreSQL**: Se a connection string contiver `Host=` ou `Server=` e `Port=`
3. **SQL Server**: Para outras connection strings

Configure a connection string no `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=ProductsCatalog;Username=user;Password=pass"
  }
}
```

## 🧪 Testes

Para executar os testes unitários:

1. Abra o **Test Explorer** no Visual Studio (`Ctrl + E, T`)
2. Clique em **Run All Tests** ou execute testes individuais
3. Os resultados serão exibidos no Test Explorer

Os testes validam as regras de negócio e o comportamento dos Application Services.

## 📝 Notas Adicionais

- O projeto utiliza autenticação JWT. É necessário gerar um token antes de acessar os endpoints protegidos
- A API está documentada via Swagger, facilitando a exploração e teste dos endpoints
- As entidades de domínio contêm validações que garantem a integridade dos dados
