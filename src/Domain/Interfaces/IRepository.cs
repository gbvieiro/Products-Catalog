using Shared.Entities;

namespace Products.Catalog.Domain.Interfaces;

public interface IRepository<EntityType> where EntityType : Entity
{
    Task CreateAsync(EntityType entity);
    Task<EntityType?> ReadAsync(Guid id);
    Task UpdateAsync(Guid id, EntityType entity);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<EntityType>> FindAsync(string filter, int skip, int take);
}