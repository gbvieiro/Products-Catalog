using Products.Catalog.Domain.Entities.Books;
using Products.Catalog.Domain.Entities.Orders;
using Products.Catalog.Domain.RepositoriesInterfaces;
using Products.Catalog.Domain.Validations;

namespace Products.Catalog.Domain.Services.Stock
{
    public class OrderDomainService : IOrderDomainService
    {
        private IBookRepository _bookRepository;
        private IOrderRepository _orderRepository;

        public OrderDomainService(
            IBookRepository bookRepository,
            IOrderRepository orderRepository
        )
        {
            _bookRepository = bookRepository;
            _orderRepository = orderRepository;
        }

        /// <summary>
        /// This method process a new order.
        /// </summary>
        /// <param name="order">A order.</param>
        public async Task ProcessNewOrderAsync(Order order)
        {
            order.SetStatus(OrderStatusEnum.Created);

            var booksToUpdate = new List<Book>();

            // Reserve all items in the stock.
            foreach (var OrderItem in order.Items)
            {
                var book =
                    await _bookRepository.GetAsync(OrderItem.BookId) ??
                        throw new DomainExceptionValidation(
                            $"Invalid book in the order: {OrderItem.BookId}"
                        );

                book.ReserveItemsFromStock(OrderItem.Quantity);
                OrderItem.UpdateAmount(book.Price);

                booksToUpdate.Add(book);
            }

            // Everhing is ok? Save all books stock.
            var tasks = new List<Task>();
            booksToUpdate.ForEach(book => { 
                tasks.Add(_bookRepository.SaveAsync(book)); 
            });

            // Run all saves.
            await Task.WhenAll(tasks);

            order.UpdateTotalAmount();

            // Save order with new state.
            await _orderRepository.SaveAsync(order);
        }

        /// <summary>
        /// This method will cancel a order.
        /// </summary>
        /// <param name="order">A order.</param>
        public async Task CancelOrderAsync(Order order)
        {
            order.SetStatus(OrderStatusEnum.Canceled);

            var booksToUpdate = new List<Book>();

            // Return all items to stock.
            foreach (var OrderItem in order.Items)
            {
                var book = await _bookRepository.GetAsync(OrderItem.BookId) ??
                    throw new DomainExceptionValidation(
                        $"Invalid book in the order: {OrderItem.BookId}"
                    );

                book.AddItemsToStock(OrderItem.Quantity);

                booksToUpdate.Add(book);
            }

            var tasks = new List<Task>();
            foreach (var book in booksToUpdate)
            {
                tasks.Add(_bookRepository.SaveAsync(book));
            }

            // Run all saves.
            await Task.WhenAll(tasks);

            // Update order information
            await _orderRepository.SaveAsync(order);
        }
    }
}