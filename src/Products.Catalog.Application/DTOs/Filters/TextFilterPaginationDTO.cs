using Products.Catalog.Application.DTOs.Pagination;

namespace Products.Catalog.Application.DTOs.Filters
{
    public class TextFilterPaginationDTO  : PaginationDto
    {
        public string? Text { get; set; }
    }
}