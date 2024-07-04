using Products.Catalog.Domain.Entities.Orders;
using Products.Catalog.Domain.Interfaces;
using Products.Catalog.Domain.Validations;

namespace Products.Catalog.Domain.Services.Orders
{
    public class OrderDomainService(
        IBooksRepository bookRepository,
        IOrderRepository orderRepository,
        IStocksRepository stockRepository
    ) : IOrderDomainService
    {
        private readonly IBooksRepository _bookRepository = bookRepository;
        
        private readonly IOrderRepository _orderRepository = orderRepository;
        
        private readonly IStocksRepository _stockRepository = stockRepository;

        public async Task ProcessNewOrderAsync(Order order)
        {
            order.SetStatus(OrderStatusEnum.Created);

            var reserveBooksTasks = new List<Task>();

            var verificationResult = await VerifyStocksForOrdemItems(order.Items);
            if(verificationResult.Length > 0)
            {
                throw new DomainExceptionValidation(string.Join(".", verificationResult));
            }

            foreach (var orderItem in order.Items)
            {
                reserveBooksTasks.Add(ReserveBookFromStock(orderItem.BookId, orderItem.Quantity));

                var price = await _bookRepository.GetBookPrice(orderItem.BookId);
                orderItem.UpdateAmount(price);
            }

            await Task.WhenAll(reserveBooksTasks);

            order.UpdateTotalAmount();

            await _orderRepository.SaveAsync(order);
        }

        private async Task<string[]> VerifyStocksForOrdemItems(List<OrderItem> orderItem)
        {
            var responses = new List<string>();
            foreach (var item in orderItem)
            {
                var stock = await _stockRepository.GetByBookId(item.BookId);

                if (stock == null)
                {
                    responses.Add($"No Stock found for book: {item.BookId}") ;
                }
                else if (stock.Quantity < item.Quantity)
                {
                    responses.Add($"No Enough Items in Stock for book:{item.BookId}");
                }
            }

            return responses.ToArray();
        }

        public async Task AddBookToStock(Guid bookId, int quantity)
        {
            
        }

        public async Task ReserveBookFromStock(Guid bookId, int quantity)
        {
            var stock = await _stockRepository.GetByBookId(bookId);
            if (stock == null)
            {
                throw new DomainExceptionValidation($"No stock found for book:{bookId}");
            }

            stock.ReserveItems(quantity);

            await _stockRepository.SaveAsync(stock);
        }
    }
}