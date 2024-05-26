using Products.Catalog.Domain.Entities.Base;

namespace Products.Catalog.Domain.RepositoriesInterfaces.Common
{
    public interface IRepository<EntityType, IdType> where EntityType : IEntity<IdType>
    {
        IEnumerable<EntityType> GetAll(string filter, int skip, int take);
        EntityType? Get(IdType id);
        void Save(EntityType entity);
        void Delete(IdType id);
    }
}