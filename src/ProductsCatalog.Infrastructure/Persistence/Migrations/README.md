# Migrations

O modelo mudou bastante nesta refatoracao (Configurations separadas por
entidade, `Amount` de `OrderItem` virou calculado/ignorado, `PasswordHash`
no lugar de `Password`, `UpdatedAt` no lugar de `UpdateAt`, etc.), entao a
migration antiga (`InitialCreate`, que estava em `src/Infra/Migrations`) foi
descartada em vez de adaptada — ela nao bate mais com o modelo novo.

Este ambiente nao tem o `dotnet` CLI disponivel para gerar uma migration
nova e validar o build, entao a pasta fica vazia por enquanto. Para gerar a
primeira migration deste modelo, na maquina com o SDK do .NET instalado:

```bash
cd src
dotnet ef migrations add InitialCreate \
  --project ProductsCatalog.Infrastructure \
  --startup-project ProductsCatalog.Api

dotnet ef database update \
  --project ProductsCatalog.Infrastructure \
  --startup-project ProductsCatalog.Api
```

Sem `ConnectionStrings:DefaultConnection` configurada, a API usa banco
InMemory automaticamente (nao precisa de migration para rodar/testar).
