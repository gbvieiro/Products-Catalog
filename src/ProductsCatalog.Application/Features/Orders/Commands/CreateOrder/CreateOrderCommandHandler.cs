using MediatR;
using ProductsCatalog.Application.Common.Exceptions;
using ProductsCatalog.Domain.Entities;
using ProductsCatalog.Domain.Repositories;

namespace ProductsCatalog.Application.Features.Orders.Commands.CreateOrder;

/// <summary>
/// Caso de uso que toca dois agregados (Order e Stock) numa unica
/// transacao: reserva o estoque de cada item e so entao cria o pedido.
/// Se qualquer reserva falhar (estoque insuficiente), a excecao de dominio
/// propaga e nada e persistido (o UnitOfWorkBehavior so faz commit se o
/// handler terminar sem excecao).
/// </summary>
public sealed class CreateOrderCommandHandler(
    IOrderRepository orderRepository,
    IBookRepository bookRepository,
    IStockRepository stockRepository,
    ICustomerRepository customerRepository) : IRequestHandler<CreateOrderCommand, Guid>
{
    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        _ = await customerRepository.GetByIdAsync(request.CustomerId, cancellationToken)
            ?? throw new NotFoundException(nameof(Customer), request.CustomerId);

        var orderItems = new List<OrderItem>();

        foreach (var item in request.Items)
        {
            var book = await bookRepository.GetByIdAsync(item.BookId, cancellationToken)
                ?? throw new NotFoundException(nameof(Book), item.BookId);

            var stock = await stockRepository.GetByBookIdAsync(item.BookId, cancellationToken)
                ?? throw new NotFoundException(nameof(Stock), item.BookId);

            stock.Reserve(item.Quantity);
            stockRepository.Update(stock);

            orderItems.Add(new OrderItem(item.BookId, item.Quantity, book.Price));
        }

        var order = new Order(request.CustomerId, orderItems);
        await orderRepository.AddAsync(order, cancellationToken);

        return order.Id;
    }
}
