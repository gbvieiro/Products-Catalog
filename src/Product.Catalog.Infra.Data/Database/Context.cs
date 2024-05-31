using Products.Catalog.Domain.Entities.Books;
using Products.Catalog.Domain.Entities.Orders;
using Products.Catalog.Domain.Entities.Stocks;
using Products.Catalog.Domain.Entities.Users;

namespace Product.Catalog.Infra.Data.Database
{
    /// <summary>
    /// To simplify running this project for analyse all data is saved in memory.
    /// </summary>
    public static class Context
    {
        /// <summary>
        /// Static constructor.
        /// </summary>
        static Context()
        {
            Books = [];
            Orders = [];
            Stocks = [];
            Users = [];
        }

        /// <summary>
        /// Storage books in memory.
        /// </summary>
        public static List<Book> Books { get; set; }

        /// <summary>
        /// Storage orders in memory.
        /// </summary>
        public static List<Order> Orders { get; set; }

        /// <summary>
        /// Storage stocks in memory.
        /// </summary>
        public static List<Stock> Stocks { get; set; }

        /// <summary>
        /// Storage users in memory.
        /// </summary>
        public static List<User> Users { get; set; }
    }
}