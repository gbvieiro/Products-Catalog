using Products.Catalog.Application.DTOs.Common;

namespace Products.Catalog.Application.DTOs
{
    public class BookDto : ProductDTO
    {
        public required string Title { get; set; }

        public required string Author { get; set; }

        public required int Genre { get; set; }
    }
}