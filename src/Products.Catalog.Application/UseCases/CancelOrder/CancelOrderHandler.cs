using MediatR;
using Products.Catalog.Domain.Entities.Orders;
using Products.Catalog.Domain.Interfaces;
using Products.Catalog.Domain.Validations;

namespace Products.Catalog.Application.UseCases.CancelOrder
{
    public class CancelOrderHandler(IOrderRepository orderRepository, IStocksRepository stockRepository) : IRequestHandler<CancelOrderRequest, CancelOrderResponse>
    {
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly IStocksRepository _stockRepository = stockRepository;

        public async Task<CancelOrderResponse> Handle(CancelOrderRequest request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetAsync(request.OrderId);

            if(order == null)
            {
                return new CancelOrderResponse()
                {
                    Message = "Could not found order with informed id:",
                    StatusCode = System.Net.HttpStatusCode.NoContent
                };
            }

            order.SetStatus(OrderStatusEnum.Canceled);

            var addBooksTasks = new List<Task>();

            foreach (var orderItem in order.Items)
            {
                var bookId = orderItem.BookId;
                var quantity = orderItem.Quantity;

                addBooksTasks.Add(AddBookToStock(bookId, quantity));
            }

            await Task.WhenAll(addBooksTasks);

            await _orderRepository.SaveAsync(order);

            return new CancelOrderResponse()
            {
                Message = $"Order {request.OrderId} has been deleted",
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }

        public async Task AddBookToStock(Guid bookId, int quantity)
        {
            var stock = await _stockRepository.GetByBookId(bookId);
            if (stock == null)
            {
                throw new DomainExceptionValidation($"No stock found for book: {bookId}");
            }

            stock.AddBooksToStock(quantity);
        }
    }
}