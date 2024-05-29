using Products.Catalog.Application.DTOs;
using Products.Catalog.Application.Services.Common;

namespace Products.Catalog.Application.Services.Orders
{
    /// <summary>
    /// Provide a interface for orders domain user cases.
    /// </summary>
    public interface IOrdersAppService : ICrudAppService<OrderDto>
    {
    }
}