using Products.Catalog.Domain.Entities.Base;

namespace Products.Catalog.Domain.RepositoriesInterfaces.Common
{
    /// <summary>
    /// A interface that defines base methods for all repositories.
    /// </summary>
    /// <typeparam name="EntityType">A entity type.</typeparam>
    /// <typeparam name="IdType">A entity id type.</typeparam>
    public interface IRepository<EntityType, IdType> where EntityType : IEntity<IdType>
    {
        /// <summary>
        /// Get all entities paginated.
        /// </summary>
        /// <param name="filter">A filter text.</param>
        /// <param name="skip">A number of items to skip.</param>
        /// <param name="take">A nmumber of items to take.</param>
        /// <returns>A list of entities.</returns>
        Task<IEnumerable<EntityType>> GetAllAsync(string filter, int skip, int take);

        /// <summary>
        /// Get a entity by id.
        /// </summary>
        /// <param name="id">A entity id.</param>
        /// <returns>A entity.</returns>
        Task<EntityType?> GetAsync(IdType id);

        /// <summary>
        /// Save or update a entity
        /// </summary>
        /// <param name="entity">A entity model.</param>
        /// <returns>A task.</returns>
        Task SaveAsync(EntityType entity);

        /// <summary>
        /// Delete a entity by id.
        /// </summary>
        /// <param name="id">A entity id.</param>
        /// <returns>A task.</returns>
        Task DeleteAsync(IdType id);
    }
}