using MediatR;

namespace ProductsCatalog.Domain.Events;

/// <summary>
/// Base para todo Domain Event. Implementa INotification para que a
/// Infrastructure (ApplicationDbContext) possa publica-lo via MediatR logo
/// apos o SaveChanges ter sucesso.
/// </summary>
public abstract class BaseDomainEvent : INotification
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
