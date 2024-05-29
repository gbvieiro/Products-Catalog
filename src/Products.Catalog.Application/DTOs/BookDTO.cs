using Products.Catalog.Application.DTOs.Common;

namespace Products.Catalog.Application.DTOs
{
    public class BookDto : ProductDTO
    {
        /// <summary>
        /// Title of the book.
        /// </summary>
        public required string Title { get; set; }

        /// <summary>
        /// Author of the book.
        /// </summary>
        public required string Author { get; set; }

        /// <summary>
        /// The genre of the book.
        /// </summary>
        public required int Genre { get; set; }
    }
}