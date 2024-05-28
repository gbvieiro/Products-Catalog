using FluentAssertions;
using Moq;
using Products.Catalog.Domain.Entities.Books;
using Products.Catalog.Domain.Entities.Orders;
using Products.Catalog.Domain.RepositoriesInterfaces;
using Products.Catalog.Domain.Services.Stock;
using Products.Catalog.Domain.Validations;
using Xunit;

namespace Products.Catalog.Domain.Tests.Services
{
    public class OrderDomainServiceTests
    {
        private IOrderDomainService _orderDomainService;
        private Mock<IBookRepository> _bookRepositoryMock;
        private Mock<IOrderRepository> _orderRepositoryMock;
        private List<Book> _books { get; set; }
        private List<Order> _orders { get; set; }

        public OrderDomainServiceTests() 
        {
            _books = new List<Book>();
            _orders = new List<Order>();

            _bookRepositoryMock = new Mock<IBookRepository>();
            _orderRepositoryMock = new Mock<IOrderRepository>();

            _bookRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>())).Returns((Guid id) =>
            {
                return Task.FromResult(_books.Where(book => book.Id == id).FirstOrDefault());
            });

            _bookRepositoryMock.Setup(x => x.SaveAsync(It.IsAny<Book>())).Returns((Book book) =>
            {
                var currentBook = _books.Where(b => b.Id == book.Id).FirstOrDefault();
                if(currentBook != null)
                {
                    currentBook = book;
                }
                else
                {
                    _books.Add(book);
                }

                return Task.CompletedTask;
            });

            _orderRepositoryMock.Setup(x => x.SaveAsync(It.IsAny<Order>())).Returns((Order order) =>
            {
                var currentOrder = _orders.Where(o => o.Id == order.Id).FirstOrDefault();
                if (currentOrder != null)
                {
                    currentOrder = order;
                }
                else
                {
                    _orders.Add(order);
                }

                return Task.CompletedTask;
            });

            _orderDomainService = new OrderDomainService(
                _bookRepositoryMock.Object,
                _orderRepositoryMock.Object
            );
        }

        [Fact]
        public async Task ProcessOrder_MustUpdateBooksStock()
        {
            var book1ID = Guid.NewGuid();
            var book2ID = Guid.NewGuid();

            _books =
            [
                new Book(book1ID, 22.50, 5, "The Shining", "Stephen King", BookGenre.Horror),
                new Book(book2ID, 17.00, 25, "Pet Semetary", "Stephen King", BookGenre.Horror),
            ];

            var orderItems = new List<OrderItem>
            {
                new(_books[0].Id, 2, 0),
                new(_books[1].Id, 10, 0)
            };

            var orderId = Guid.NewGuid();
            var orderDate = DateTime.Now;

            var order = new Order(
                orderId, orderDate, OrderStatusEnum.Created,
                orderItems, 0
            );

            await _orderDomainService.ProcessNewOrderAsync(order);

            // Assert order status
            Xunit.Assert.Equal(OrderStatusEnum.Created, order.Status);
            Xunit.Assert.Equal(book1ID, order.Items[0].BookId);
            Xunit.Assert.Equal(45, order.Items[0].Amount);
            Xunit.Assert.Equal(book2ID, order.Items[1].BookId);
            Xunit.Assert.Equal(170, order.Items[1].Amount);
            Xunit.Assert.Equal(215, order.TotalAmount);

            // Assert book 1
            Xunit.Assert.Equal(book1ID, _books[0].Id);
            Xunit.Assert.Equal("The Shining", _books[0].Title);
            Xunit.Assert.Equal("Stephen King", _books[0].Author);
            Xunit.Assert.Equal(BookGenre.Horror, _books[0].Genre);
            Xunit.Assert.Equal(22.50, _books[0].Price);
            Xunit.Assert.Equal(3, _books[0].StockQuantity);

            // Assert book 2
            Xunit.Assert.Equal(book2ID, _books[1].Id);
            Xunit.Assert.Equal("Pet Semetary", _books[1].Title);
            Xunit.Assert.Equal("Stephen King", _books[1].Author);
            Xunit.Assert.Equal(BookGenre.Horror, _books[1].Genre);
            Xunit.Assert.Equal(17.00, _books[1].Price);
            Xunit.Assert.Equal(15, _books[1].StockQuantity);
        }

        [Fact]
        public async Task ProcessOrder_WithInvalidQuantity_DomainException()
        {
            var book1ID = Guid.NewGuid();
            var book2ID = Guid.NewGuid();

            _books =
            [
                new Book(book1ID, 22.50, 5, "The Shining", "Stephen King", BookGenre.Horror),
                new Book(book2ID, 17.00, 25, "Pet Semetary", "Stephen King", BookGenre.Horror),
            ];

            var orderItems = new List<OrderItem>
            {
                new(_books[0].Id, 6, 0),
                new(_books[1].Id, 10, 0)
            };

            var orderId = Guid.NewGuid();
            var orderDate = DateTime.Now;

            var order = new Order(
                orderId, orderDate, OrderStatusEnum.Created,
                orderItems, 0
            );

            Func<Task> func = async () => await _orderDomainService.ProcessNewOrderAsync(order);
            await func.Should().ThrowAsync<DomainExceptionValidation>()
                .WithMessage($"Not enough stock of The Shining, available stock: 5.");
        }

        [Fact]
        public async Task CancelOrder_MustUpdateBooksStock()
        {
            var book1ID = Guid.NewGuid();
            var book2ID = Guid.NewGuid();

            _books =
            [
                new Book(book1ID, 22.50, 5, "The Shining", "Stephen King", BookGenre.Horror),
                new Book(book2ID, 17.00, 25, "Pet Semetary", "Stephen King", BookGenre.Horror),
            ];

            var orderItems = new List<OrderItem>
            {
                new(_books[0].Id, 2, 0),
                new(_books[1].Id, 10, 0)
            };

            var order = new Order(
                Guid.NewGuid(),
                DateTime.Now,
                OrderStatusEnum.Created,
                orderItems,
                0
            );

            await _orderDomainService.CancelOrderAsync(order);

            // Assert order status
            Xunit.Assert.Equal(OrderStatusEnum.Canceled, order.Status);

            // Assert book 1
            Xunit.Assert.Equal(book1ID, _books[0].Id);
            Xunit.Assert.Equal("The Shining", _books[0].Title);
            Xunit.Assert.Equal("Stephen King", _books[0].Author);
            Xunit.Assert.Equal(BookGenre.Horror, _books[0].Genre);
            Xunit.Assert.Equal(22.50, _books[0].Price);
            Xunit.Assert.Equal(7, _books[0].StockQuantity);

            // Assert book 2
            Xunit.Assert.Equal(book2ID, _books[1].Id);
            Xunit.Assert.Equal("Pet Semetary", _books[1].Title);
            Xunit.Assert.Equal("Stephen King", _books[1].Author);
            Xunit.Assert.Equal(BookGenre.Horror, _books[1].Genre);
            Xunit.Assert.Equal(17.00, _books[1].Price);
            Xunit.Assert.Equal(35, _books[1].StockQuantity);
        }
    }
}
