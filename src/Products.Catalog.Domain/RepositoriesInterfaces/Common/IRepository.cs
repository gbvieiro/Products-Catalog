using Products.Catalog.Domain.Entities.Base;

namespace Products.Catalog.Domain.RepositoriesInterfaces.Common
{
    public interface IRepository<EntityType, IdType> where EntityType : IEntity<IdType>
    {
        EntityType GetAll(int skip, int take);
        EntityType Get(IdType id);
        void Save(EntityType entity);
        void Delete(IdType id);
    }
}