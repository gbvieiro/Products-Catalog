using MediatR;
using Products.Catalog.Domain.Entities;
using Products.Catalog.Domain.Enums;
using Products.Catalog.Domain.Interfaces;
using Products.Catalog.Domain.Validations;

namespace Products.Catalog.Application.UseCases.CancelOrder;

public class CancelOrderHandler(IRepository<Order> orderRepository, IRepository<Stock> stockRepository) : IRequestHandler<CancelOrderRequest, CancelOrderResponse>
{
    private readonly IRepository<Order> _orderRepository = orderRepository;
    private readonly IRepository<Stock> _stockRepository = stockRepository;

    public async Task<CancelOrderResponse> Handle(CancelOrderRequest request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.ReadAsync(request.OrderId);

        if(order == null)
        {
            return new CancelOrderResponse()
            {
                Message = "Could not found order with informed id:",
                StatusCode = System.Net.HttpStatusCode.NoContent
            };
        }

        order.SetStatus(EOrderStatus.Canceled);

        var addBooksTasks = new List<Task>();

        foreach (var orderItem in order.Items)
        {
            var bookId = orderItem.BookId;
            var quantity = orderItem.Quantity;

            addBooksTasks.Add(AddBookToStock(bookId, quantity));
        }

        await Task.WhenAll(addBooksTasks);

        await _orderRepository.UpdateAsync(order.Id, order);

        return new CancelOrderResponse()
        {
            Message = $"Order {request.OrderId} has been deleted",
            StatusCode = System.Net.HttpStatusCode.OK
        };
    }

    public async Task AddBookToStock(Guid bookId, int quantity)
    {
        var allStocks = await _stockRepository.FindAsync(string.Empty, 0, int.MaxValue);
        var stock = allStocks.FirstOrDefault(s => s.BookId == bookId);
        
        if (stock == null)
        {
            throw new DomainException($"No stock found for book: {bookId}");
        }

        stock.AddBooksToStock(quantity);
        await _stockRepository.UpdateAsync(stock.Id, stock);
    }
}