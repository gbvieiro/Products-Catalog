using Products.Catalog.Application.DTOs.Pagination;

namespace Products.Catalog.Application.DTOs.Filters
{
    public class TextFilterPaginationDTO(string text) : PaginationDTO
    {
        public string Text { get; set; } = text;
    }
}