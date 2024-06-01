using Products.Catalog.Domain.Entities.Orders;
using Products.Catalog.Domain.RepositoriesInterfaces;
using Products.Catalog.Domain.Validations;

namespace Products.Catalog.Domain.Services.Orders
{
    /// <summary>
    /// A order domain service. 
    /// Provide some important rules for orders management.
    /// </summary>
    /// <param name="bookRepository">A book repository instance.</param>
    /// <param name="orderRepository">A order repository instance.</param>
    /// <param name="stockRepository">A stock repository instance.</param>
    public class OrderDomainService(
        IBooksRepository bookRepository,
        IOrderRepository orderRepository,
        IStocksRepository stockRepository
    ) : IOrderDomainService
    {
        /// <summary>
        /// A books repository interface.
        /// </summary>
        private readonly IBooksRepository _bookRepository = bookRepository;
        
        /// <summary>
        /// A orders repository interface.
        /// </summary>
        private readonly IOrderRepository _orderRepository = orderRepository;
        
        /// <summary>
        /// A stocks repository interface.
        /// </summary>
        private readonly IStocksRepository _stockRepository = stockRepository;

        /// <inheritdoc/>
        public async Task ProcessNewOrderAsync(Order order)
        {
            order.SetStatus(OrderStatusEnum.Created);

            // Return all items to stock.
            var reserveBooksTasks = new List<Task>();

            var verificationResult = await VerifyStocksForOrdemItems(order.Items);
            if(verificationResult.Length > 0)
            {
                throw new DomainExceptionValidation(string.Join(".", verificationResult));
            }

            // Reserve all items in the stock.
            foreach (var orderItem in order.Items)
            {
                // Create a task to run in parallel.
                reserveBooksTasks.Add(ReserveBookFromStock(orderItem.BookId, orderItem.Quantity));

                // Update order amount.
                var price = await _bookRepository.GetBookPrice(orderItem.BookId);
                orderItem.UpdateAmount(price);
            }

            // Run all saves.
            await Task.WhenAll(reserveBooksTasks);

            order.UpdateTotalAmount();

            // Save order with new state.
            await _orderRepository.SaveAsync(order);
        }

        /// <summary>
        /// This verification avoid a order to have more items than what we have in stock.
        /// </summary>
        /// <param name="orderItem">A order item.</param>
        /// <returns>A list of problems with stock or a empty list when ok.</returns>
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

        /// <summary>
        /// Add books to the stock.
        /// </summary>
        /// <param name="bookId">Book id.</param>
        /// <param name="quantity">Quantity id.</param>
        /// <returns>A task.</returns>
        /// <exception cref="DomainExceptionValidation">When there is no stock register.</exception>
        private async Task AddBookToStock(Guid bookId, int quantity)
        {
            var stock = await _stockRepository.GetByBookId(bookId);
            if(stock == null)
            {
                throw new DomainExceptionValidation($"No stock found for book:{bookId}");
            }

            stock.AddBooksToStock(quantity);
        }

        /// <summary>
        /// Reserve books from the stock.
        /// </summary>
        /// <param name="bookId">Book id.</param>
        /// <param name="quantity">Quantity id.</param>
        /// <returns>A task.</returns>
        private async Task ReserveBookFromStock(Guid bookId, int quantity)
        {
            var stock = await _stockRepository.GetByBookId(bookId);
            if (stock == null)
            {
                throw new DomainExceptionValidation($"No stock found for book:{bookId}");
            }

            stock.ReserveItems(quantity);

            await _stockRepository.SaveAsync(stock);
        }

        /// <inheritdoc/>
        public async Task CancelOrderAsync(Order order)
        {
            order.SetStatus(OrderStatusEnum.Canceled);

            // Return all items to stock.
            var addBooksTasks = new List<Task>();
            foreach (var orderItem in order.Items)
            {
                // Create a task to run in parallel.
                addBooksTasks.Add(AddBookToStock(orderItem.BookId, orderItem.Quantity));
            }

            // Run in parallel
            await Task.WhenAll(addBooksTasks);

            // Update order information
            await _orderRepository.SaveAsync(order);
        }
    }
}