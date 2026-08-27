using ProductsCatalog.Domain.Events;

namespace ProductsCatalog.Domain.Common;

/// <summary>
/// Classe base para todas as entidades do dominio. Concentra identidade,
/// auditoria basica (CreatedAt/UpdatedAt) e o mecanismo de Domain Events.
/// </summary>
public abstract class BaseEntity
{
    private readonly List<BaseDomainEvent> _domainEvents = [];

    protected BaseEntity()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public IReadOnlyCollection<BaseDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(BaseDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    public void ClearDomainEvents() => _domainEvents.Clear();

    /// <summary>Marca a entidade como alterada agora. Chame ao final de qualquer metodo de mutacao.</summary>
    protected void Touch() => UpdatedAt = DateTime.UtcNow;
}
