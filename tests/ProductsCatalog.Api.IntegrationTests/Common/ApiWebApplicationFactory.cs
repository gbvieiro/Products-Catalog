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
        using var scope = host.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        context.Database.EnsureCreated();

        return host;
    }
}
