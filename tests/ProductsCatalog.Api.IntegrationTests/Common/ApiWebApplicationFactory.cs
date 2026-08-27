using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductsCatalog.Infrastructure.Persistence;

namespace ProductsCatalog.Api.IntegrationTests.Common;

/// <summary>
/// Sobe a Api inteira (Program.cs real, DI real). Por padrao troca o banco
/// por um InMemory com nome unico por instancia de factory, para que os
/// testes nao vazem estado entre si. Se a variavel de ambiente
/// ConnectionStrings__DefaultConnection estiver definida (ex: quando o
/// docker-compose sobe um Postgres real para os testes), ela e respeitada
/// e a Api roda contra o banco real - assim os mesmos testes servem tanto
/// para feedback rapido local (InMemory) quanto para uma validacao mais
/// realista via docker-compose (Postgres).
/// </summary>
public class ApiWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _databaseName = $"IntegrationTests-{Guid.NewGuid()}";

    // xUnit roda classes de teste em paralelo por padrao, e cada classe de
    // integracao cria a sua propria ApiWebApplicationFactory. Quando o banco
    // real (Postgres) e usado - via ConnectionStrings__DefaultConnection,
    // como no docker-compose - todas as instancias apontam para o MESMO
    // banco fisico. EnsureCreated() nao e thread-safe/process-safe: duas
    // factories chamando-o ao mesmo tempo podem colidir tentando criar os
    // mesmos tipos/tabelas, causando erros como
    // "duplicate key value violates unique constraint pg_type_typname_nsp_index".
    // O semaforo estatico serializa a criacao do schema entre todas as
    // instancias do processo de teste; como EnsureCreated() e idempotente,
    // as chamadas subsequentes apenas confirmam que o schema ja existe.
    private static readonly SemaphoreSlim SchemaCreationLock = new(1, 1);

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var hasRealConnectionString = !string.IsNullOrWhiteSpace(
            Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection"));

        if (hasRealConnectionString)
        {
            return;
        }

        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase(_databaseName));
        });
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = base.CreateHost(builder);

        // Nao ha migrations geradas ainda (ver tests/.../Persistence/Migrations/README.md),
        // entao criamos o schema direto a partir do modelo atual. Funciona tanto para
        // InMemory quanto para um Postgres/SqlServer real.
        //
        // O lock abaixo evita a corrida entre factories concorrentes descrita
        // no comentario de SchemaCreationLock. Para o InMemory ele e inofensivo
        // (cada factory tem seu proprio banco, entao nunca ha contencao real),
        // mas mante-lo tambem nesse caminho simplifica o codigo.
        SchemaCreationLock.Wait();
        try
        {
            using var scope = host.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.EnsureCreated();
        }
        finally
        {
            SchemaCreationLock.Release();
        }

        return host;
    }
}
