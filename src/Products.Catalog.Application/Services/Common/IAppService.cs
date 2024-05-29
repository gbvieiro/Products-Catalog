using Products.Catalog.Application.DTOs.Common;

namespace Products.Catalog.Application.Services.Common
{
    public interface IAppService<DtoType> where DtoType : EntityDTO
    {
        /// <summary>
        /// Get a entity by id.
        /// </summary>
        /// <param name="id">A entity id.</param>
        /// <returns>A entity dto.</returns>
        Task<DtoType?> GetAsync(Guid id);

        /// <summary>
        /// Get all entities using pagination.
        /// </summary>
        /// <param name="filtertext">A filter text.</param>
        /// <param name="skip">A number of entities to skip.</param>
        /// <param name="take">A number of entities to take.</param>
        /// <returns>A list of entities.</returns>
        Task<List<DtoType>> GetAllAsync(string filtertext, int skip, int take);

        /// <summary>
        /// Save a entity.
        /// </summary>
        /// <param name="book">A entity dto.</param>
        Task SaveAsync(DtoType book);

        /// <summary>
        /// Delete a entity.
        /// </summary>
        /// <param name="book">A book entity.</param>
        Task DeleteAsync(Guid id);
    }
}