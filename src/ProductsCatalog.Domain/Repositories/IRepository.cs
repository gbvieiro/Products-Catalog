using ProductsCatalog.Domain.Common;
using ProductsCatalog.Domain.Specifications;

namespace ProductsCatalog.Domain.Repositories;

/// <summary>
/// Porta generica de persistencia. Note que Update/Remove sao sincronos:
/// eles apenas marcam o estado no change tracker. A escrita efetiva no banco
/// (o "commit") acontece em um unico ponto, via IUnitOfWork.SaveChangesAsync,
/// disparado automaticamente pelo UnitOfWorkBehavior apos o handler de um
/// Command terminar com sucesso.
/// </summary>
public interface IRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<T>> ListAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

    Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

    Task AddAsync(T entity, CancellationToken cancellationToken = default);

    void Update(T entity);

    void Remove(T entity);
}
