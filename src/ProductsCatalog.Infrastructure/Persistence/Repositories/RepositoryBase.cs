using Microsoft.EntityFrameworkCore;
using ProductsCatalog.Domain.Common;
using ProductsCatalog.Domain.Repositories;
using ProductsCatalog.Domain.Specifications;

namespace ProductsCatalog.Infrastructure.Persistence.Repositories;

public class RepositoryBase<T>(ApplicationDbContext context) : IRepository<T> where T : BaseEntity
{
    protected readonly ApplicationDbContext Context = context;
    protected readonly DbSet<T> DbSet = context.Set<T>();

    public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await DbSet.FindAsync([id], cancellationToken);

    public virtual async Task<IReadOnlyList<T>> ListAsync(
        ISpecification<T> specification, CancellationToken cancellationToken = default) =>
        await SpecificationEvaluator.GetQuery(DbSet.AsQueryable(), specification).ToListAsync(cancellationToken);

    public virtual async Task<int> CountAsync(
        ISpecification<T> specification, CancellationToken cancellationToken = default) =>
        await SpecificationEvaluator
            .GetQuery(DbSet.AsQueryable(), specification, evaluateCriteriaOnly: true)
            .CountAsync(cancellationToken);

    public virtual async Task AddAsync(T entity, CancellationToken cancellationToken = default) =>
        await DbSet.AddAsync(entity, cancellationToken);

    public virtual void Update(T entity) => DbSet.Update(entity);

    public virtual void Remove(T entity) => DbSet.Remove(entity);
}
