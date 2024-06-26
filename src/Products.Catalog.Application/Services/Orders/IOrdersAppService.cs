using Products.Catalog.Application.DTOs;
using Products.Catalog.Application.Services.Common;

namespace Products.Catalog.Application.Services.Orders
{
    public interface IOrdersAppService : ICrudAppService<OrderDto>
    {
        Task<string> CancelAsync(Guid id);

        Task<List<OrderDto>> GetAllAsync(string filtertext, int skip, int take, Guid userId);
    }
}