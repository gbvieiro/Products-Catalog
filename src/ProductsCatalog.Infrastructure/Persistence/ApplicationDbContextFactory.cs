using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ProductsCatalog.Infrastructure.Persistence;

/// <summary>Usada pelas ferramentas de design-time do EF Core (dotnet ef migrations/database update).</summary>
public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        optionsBuilder.UseNpgsql(
            "Host=localhost;Port=5432;Database=product_catalog;Username=postgres;Password=admin",
            b => b.MigrationsAssembly("ProductsCatalog.Infrastructure"));

        // Em design-time nao existe container de DI, entao passamos um IPublisher
        // "no-op": ele nunca e usado de fato para publicar eventos aqui, so
        // satisfaz o construtor do ApplicationDbContext.
        return new ApplicationDbContext(optionsBuilder.Options, new DesignTimePublisher());
    }

    private sealed class DesignTimePublisher : IPublisher
    {
        public Task Publish(object notification, CancellationToken cancellationToken = default) =>
            Task.CompletedTask;

        public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
            where TNotification : INotification =>
            Task.CompletedTask;
    }
}
