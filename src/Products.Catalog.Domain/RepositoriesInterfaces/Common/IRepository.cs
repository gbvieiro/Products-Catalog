using Products.Catalog.Domain.Entities.Base;

namespace Products.Catalog.Domain.RepositoriesInterfaces.Common
{
    public interface IRepository<EntityType, IdType> where EntityType : IEntity<IdType>
    {
        Task<IEnumerable<EntityType>> GetAllAsync(string filter, int skip, int take);

        Task<EntityType?> GetAsync(IdType id);

        Task SaveAsync(EntityType entity);

        Task DeleteAsync(IdType id);
    }
}