using MediatR;
using ProductsCatalog.Application.Common.Exceptions;
using ProductsCatalog.Domain.Entities;
using ProductsCatalog.Domain.Repositories;

namespace ProductsCatalog.Application.Features.Orders.Commands.CancelOrder;

/// <summary>Cancela o pedido e devolve os itens para o estoque.</summary>
public sealed class CancelOrderCommandHandler(
    IOrderRepository orderRepository,
    IStockRepository stockRepository) : IRequestHandler<CancelOrderCommand, CancelOrderResult>
{
    public async Task<CancelOrderResult> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByIdAsync(request.OrderId, cancellationToken)
            ?? throw new NotFoundException(nameof(Order), request.OrderId);

        order.Cancel();
        orderRepository.Update(order);

        foreach (var item in order.Items)
        {
            var stock = await stockRepository.GetByBookIdAsync(item.BookId, cancellationToken);
            if (stock is null)
            {
                continue;
            }

            stock.Replenish(item.Quantity);
            stockRepository.Update(stock);
        }

        return new CancelOrderResult($"Order {order.Id} has been canceled.");
    }
}
