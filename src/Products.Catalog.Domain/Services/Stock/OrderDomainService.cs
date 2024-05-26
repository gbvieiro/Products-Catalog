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
        public void ProcessNewOrder(Order order)
        {
            order.SetStatus(OrderStatusEnum.Created);

            var booksToUpdate = new List<Book>();

            // Reserve all items in the stock.
            order.Items.ForEach(OrderItem => {
                var book = 
                    _bookRepository.Get(OrderItem.BookId) ?? 
                        throw new DomainExceptionValidation(
                            $"Invalid book in the order: {OrderItem.BookId}"
                        );

                book.ReserveItemsFromStock(OrderItem.Quantity);
                OrderItem.UpdateAmount(book.Price); 

                booksToUpdate.Add(book);
            });

            // Everhing is ok? Save all books stock.
            booksToUpdate.ForEach(book => { _bookRepository.Save(book); });

            order.UpdateTotalAmount();

            // Save order with new state.
            _orderRepository.Save(order);
        }

        /// <summary>
        /// This method will cancel a order.
        /// </summary>
        /// <param name="order">A order.</param>
        public void CancelOrder(Order order)
        {
            order.SetStatus(OrderStatusEnum.Canceled);

            var booksToUpdate = new List<Book>();

            // Return all items to stock.
            order.Items.ForEach(OrderItem => {
                var book =
                    _bookRepository.Get(OrderItem.BookId) ??
                        throw new DomainExceptionValidation(
                            $"Invalid book in the order: {OrderItem.BookId}"
                        );

                book.AddItemsToStock(OrderItem.Quantity);

                booksToUpdate.Add(book);
            });

            // Everhing is ok? Save all books stock.
            booksToUpdate.ForEach(book => { _bookRepository.Save(book); });

            // Update order information
            _orderRepository.Save(order);
        }
    }
}