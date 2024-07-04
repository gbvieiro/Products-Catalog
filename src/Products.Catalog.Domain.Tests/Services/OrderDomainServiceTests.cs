using FluentAssertions;
using Moq;
using Products.Catalog.Domain.Entities.Books;
using Products.Catalog.Domain.Entities.Orders;
using Products.Catalog.Domain.Entities.Stocks;
using Products.Catalog.Domain.Interfaces;
using Products.Catalog.Domain.Services.Orders;
using Products.Catalog.Domain.Validations;
using Xunit;

namespace Products.Catalog.Domain.Tests.Services
{
    public class OrderDomainServiceTests
    {
        private readonly IOrderDomainService _orderDomainService;
        private readonly Mock<IBooksRepository> _bookRepositoryMock;
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly Mock<IStocksRepository> _stocksRepository;
        private List<Book> _books { get; set; }
        private  List<Order> _orders { get; set; }
        private List<Stock> _stocks { get; set; }

        public OrderDomainServiceTests() 
        {
            _books = [];
            _orders = [];
            _stocks = [];

            _bookRepositoryMock = new Mock<IBooksRepository>();
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _stocksRepository = new Mock<IStocksRepository>();

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

            _bookRepositoryMock.Setup(x => x.GetBookPrice(It.IsAny<Guid>())).Returns((Guid id) =>
            {
                return Task.FromResult(_books.Where(book => book.Id == id).First().Price);
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

            _stocksRepository.Setup(x => x.SaveAsync(It.IsAny<Stock>())).Returns((Stock stock) =>
            {
                var currentStock = _stocks.Where(o => o.Id == stock.Id).FirstOrDefault();
                if (currentStock != null)
                {
                    currentStock = stock;
                }
                else
                {
                    _stocks.Add(stock);
                }

                return Task.CompletedTask;
            });

            _stocksRepository.Setup(x => x.GetByBookId(It.IsAny<Guid>())).Returns((Guid id) =>
            {
                return Task.FromResult(_stocks.Where(stock => stock.BookId == id).FirstOrDefault());
            });

            _orderDomainService = new OrderDomainService(
                _bookRepositoryMock.Object,
                _orderRepositoryMock.Object,
                _stocksRepository.Object
            );
        }

        [Fact]
        public async Task ProcessOrder_MustUpdateBooksStock()
        {
            var book1ID = Guid.NewGuid();
            var book2ID = Guid.NewGuid();

            _books =
            [
                new Book(book1ID, 22.50, "The Shining", "Stephen King", BookGenre.Horror),
                new Book(book2ID, 17.00, "Pet Semetary", "Stephen King", BookGenre.Horror),
            ];

            _stocks = [
                new Stock(Guid.NewGuid(), 10, book1ID),
                new Stock(Guid.NewGuid(), 15, book2ID),
            ];

            var orderItems = new List<OrderItem>
            {
                new(book1ID, 2, 0),
                new(book2ID, 10, 0)
            };

            var orderId = Guid.NewGuid();
            var orderDate = DateTime.Now;

            var order = new Order(orderId, Guid.NewGuid(), orderDate, OrderStatusEnum.Created, orderItems, 0);

            await _orderDomainService.ProcessNewOrderAsync(order);

            // Assert order status
            Xunit.Assert.Equal(OrderStatusEnum.Created, order.Status);
            Xunit.Assert.Equal(book1ID, order.Items[0].BookId);
            Xunit.Assert.Equal(45, order.Items[0].Amount);
            Xunit.Assert.Equal(book2ID, order.Items[1].BookId);
            Xunit.Assert.Equal(170, order.Items[1].Amount);
            Xunit.Assert.Equal(215, order.TotalAmount);

            // Assert stocks
            Xunit.Assert.Equal(8, _stocks[0].Quantity);
            Xunit.Assert.Equal(5, _stocks[1].Quantity);
        }

        [Fact]
        public async Task ProcessOrder_WithInvalidQuantity_DomainException()
        {
            var book1ID = Guid.NewGuid();
            var book2ID = Guid.NewGuid();

            _books =
            [
                new Book(book1ID, 22.50, "The Shining", "Stephen King", BookGenre.Horror),
                new Book(book2ID, 17.00, "Pet Semetary", "Stephen King", BookGenre.Horror),
            ];

            var orderItems = new List<OrderItem>
            {
                new(book1ID, 6, 0),
                new(book2ID, 10, 0)
            };

            _stocks = [
                new Stock(Guid.NewGuid(), 5, book1ID)
            ];

            var orderId = Guid.NewGuid();
            var orderDate = DateTime.Now;

            var order = new Order(orderId, Guid.NewGuid(), orderDate, OrderStatusEnum.Created, orderItems, 0);

            Func<Task> func = async () => await _orderDomainService.ProcessNewOrderAsync(order);
            await func.Should().ThrowAsync<DomainExceptionValidation>()
                .WithMessage($"No Enough Items in Stock for book:{book1ID}.No Stock found for book: {book2ID}");
        }

        [Fact]
        public async Task CancelOrder_MustUpdateBooksStock()
        {
            var book1ID = Guid.NewGuid();
            var book2ID = Guid.NewGuid();

            _books =
            [
                new Book(book1ID, 22.50, "The Shining", "Stephen King", BookGenre.Horror),
                new Book(book2ID, 17.00, "Pet Semetary", "Stephen King", BookGenre.Horror),
            ];

            var orderItems = new List<OrderItem>
            {
                new(book1ID, 2, 0),
                new(book2ID, 25, 0)
            };

            _stocks = [
                new Stock(Guid.NewGuid(), 8, book1ID),
                new Stock(Guid.NewGuid(), 5, book2ID),
            ];

            var order = 
                new Order(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, OrderStatusEnum.Created, orderItems, 0);

            await _orderDomainService.CancelOrderAsync(order);

            // Assert order status
            Xunit.Assert.Equal(OrderStatusEnum.Canceled, order.Status);

            // Assert stocks
            Xunit.Assert.Equal(10, _stocks[0].Quantity);
            Xunit.Assert.Equal(30, _stocks[1].Quantity);
        }
    }
}
