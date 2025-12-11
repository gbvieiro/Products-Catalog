using Infra.Databases;
using Microsoft.EntityFrameworkCore;
using Products.Catalog.Domain.Interfaces;
using Shared.Entities;

namespace Infra.Repository;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    public Repository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _dbSet = context.Set<TEntity>();
    }

    public virtual async Task CreateAsync(TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public virtual async Task<TEntity?> ReadAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task UpdateAsync(Guid id, TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        var existingEntity = await _dbSet.FindAsync(id);
        if (existingEntity == null)
            throw new InvalidOperationException($"Entity with id {id} not found.");

        _context.Entry(existingEntity).CurrentValues.SetValues(entity);
        await _context.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(Guid id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public virtual async Task<IEnumerable<TEntity>> FindAsync(string filter, int skip, int take)
    {
        var query = _dbSet.AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter))
        {
            query = query.Where(entity =>
                EF.Functions.Like(entity.Id.ToString(), $"%{filter}%") ||
                EF.Functions.Like(entity.CreatedAt.ToString(), $"%{filter}%")
            );
        }

        return await query
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }
}

