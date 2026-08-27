using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductsCatalog.Application.Common.Interfaces;
using ProductsCatalog.Domain.Common;
using ProductsCatalog.Domain.Entities;

namespace ProductsCatalog.Infrastructure.Persistence;

/// <summary>
/// Composition point entre EF Core e o resto da aplicacao. Implementa
/// IUnitOfWork (a assinatura de DbContext.SaveChangesAsync ja bate com a
/// interface, entao a implementacao e "de graca") e, ao salvar com sucesso,
/// publica os Domain Events acumulados nas entidades rastreadas.
/// </summary>
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IPublisher publisher)
    : DbContext(options), IUnitOfWork
{
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Stock> Stocks => Set<Stock>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entitiesWithEvents = ChangeTracker.Entries<BaseEntity>()
            .Select(entry => entry.Entity)
            .Where(entity => entity.DomainEvents.Count != 0)
            .ToList();

        var result = await base.SaveChangesAsync(cancellationToken);

        foreach (var entity in entitiesWithEvents)
        {
            var domainEvents = entity.DomainEvents.ToArray();
            entity.ClearDomainEvents();

            foreach (var domainEvent in domainEvents)
            {
                await publisher.Publish(domainEvent, cancellationToken);
            }
        }

        return result;
    }
}
