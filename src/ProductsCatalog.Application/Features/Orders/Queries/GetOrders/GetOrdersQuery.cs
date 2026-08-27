using ProductsCatalog.Application.Common.Messaging;
using ProductsCatalog.Application.Common.Models;

namespace ProductsCatalog.Application.Features.Orders.Queries.GetOrders;

/// <summary>
/// Query unica para "listar todos os pedidos" (CustomerId nulo, uso
/// administrativo) e para "meus pedidos" (CustomerId vindo do usuario
/// autenticado, setado pelo controller a partir do JWT).
/// </summary>
public sealed record GetOrdersQuery(Guid? CustomerId = null, string? Filter = null, int Skip = 0, int Take = 20)
    : IQuery<PagedResult<OrderDto>>;
