using Products.Catalog.Application.DTOs;
using Products.Catalog.Application.Services.Common;

namespace Products.Catalog.Application.Services.Orders
{
    public interface IOrdersAppService : IAppService<OrderDto>
    {
    }
}