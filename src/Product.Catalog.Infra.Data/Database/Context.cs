using Products.Catalog.Domain.Entities.Books;
using Products.Catalog.Domain.Entities.Orders;
using Products.Catalog.Domain.Entities.Stocks;
using Products.Catalog.Domain.Entities.Users;

namespace Product.Catalog.Infra.Data.Database
{
    public static class Context
    {
        static Context()
        {
            Books = [];
            Orders = [];
            Stocks = [];
            Users = [];
        }

        public static List<Book> Books { get; set; }

        public static List<Order> Orders { get; set; }

        public static List<Stock> Stocks { get; set; }

        public static List<User> Users { get; set; }
    }
}