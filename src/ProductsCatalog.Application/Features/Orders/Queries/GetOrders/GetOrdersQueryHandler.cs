using AutoMapper;
using MediatR;
using ProductsCatalog.Application.Common.Models;
using ProductsCatalog.Domain.Repositories;
using ProductsCatalog.Domain.Specifications;

namespace ProductsCatalog.Application.Features.Orders.Queries.GetOrders;

public sealed class GetOrdersQueryHandler(IOrderRepository orderRepository, IMapper mapper)
    : IRequestHandler<GetOrdersQuery, PagedResult<OrderDto>>
{
    public async Task<PagedResult<OrderDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var spec = new OrdersFilterSpecification(request.CustomerId, request.Filter, request.Skip, request.Take);

        var orders = await orderRepository.ListAsync(spec, cancellationToken);
        var total = await orderRepository.CountAsync(spec, cancellationToken);

        return new PagedResult<OrderDto>(mapper.Map<List<OrderDto>>(orders), total, request.Skip, request.Take);
    }
}
