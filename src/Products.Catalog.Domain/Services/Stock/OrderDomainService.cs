using Products.Catalog.Domain.Entities.Books;
using Products.Catalog.Domain.Entities.Orders;
using Products.Catalog.Domain.RepositoriesInterfaces;
using Products.Catalog.Domain.Validations;
using System.ComponentModel;
using System.Linq;

namespace Products.Catalog.Domain.Services.Stock
{
    public class OrderDomainService : IOrderDomainService
    {
        private IBookRepository _bookRepository;
        private IOrderRepository _orderRepository;
        private IStocksRepository _stockRepository;
        
        public OrderDomainService(
            IBookRepository bookRepository,
            IOrderRepository orderRepository,
            IStocksRepository stockRepository
        )
        {
            _bookRepository = bookRepository;
            _orderRepository = orderRepository;
            _stockRepository = stockRepository;
        }

        /// <summary>
        /// This method process a new order.
        /// </summary>
        /// <param name="order">A order.</param>
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

        /// <summary>
        /// This method will cancel a order.
        /// </summary>
        /// <param name="order">A order.</param>
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