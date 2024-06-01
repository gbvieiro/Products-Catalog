using Products.Catalog.Application.DTOs;
using Products.Catalog.Application.Services.Common;

namespace Products.Catalog.Application.Services.Orders
{
    /// <summary>
    /// Provide a interface for orders domain user cases.
    /// </summary>
    public interface IOrdersAppService : ICrudAppService<OrderDto>
    {
        /// <summary>
        /// Cancel order by id.
        /// </summary>
        /// <param name="id">Order id.</param>
        /// <returns>A task.</returns>
        Task<string> CancelAsync(Guid id);

        /// <summary>
        /// Get user entities using pagination.
        /// </summary>
        /// <param name="filtertext">A filter text.</param>
        /// <param name="skip">A number of entities to skip.</param>
        /// <param name="take">A number of entities to take.</param>
        /// <param name="userId">A user id.</param>
        /// <returns>A list of entities.</returns>
        Task<List<OrderDto>> GetAllAsync(string filtertext, int skip, int take, Guid userId);
    }
}