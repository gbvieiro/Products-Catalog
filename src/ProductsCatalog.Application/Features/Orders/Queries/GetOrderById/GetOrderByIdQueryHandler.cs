using AutoMapper;
using MediatR;
using ProductsCatalog.Domain.Repositories;

namespace ProductsCatalog.Application.Features.Orders.Queries.GetOrderById;

public sealed class GetOrderByIdQueryHandler(IOrderRepository orderRepository, IMapper mapper)
    : IRequestHandler<GetOrderByIdQuery, OrderDto?>
{
    public async Task<OrderDto?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByIdAsync(request.Id, cancellationToken);
        return order is null ? null : mapper.Map<OrderDto>(order);
    }
}
