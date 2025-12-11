using MediatR;

namespace Products.Catalog.Application.UseCases.CancelOrder;

public sealed record CancelOrderRequest(Guid OrderId) : IRequest<CancelOrderResponse>
{   
}