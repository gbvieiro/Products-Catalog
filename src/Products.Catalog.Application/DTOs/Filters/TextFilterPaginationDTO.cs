using Products.Catalog.Application.DTOs.Pagination;

namespace Products.Catalog.Application.DTOs.Filters
{
    /// <summary>
    /// A text filter pagination. 
    /// </summary>
    public class TextFilterPaginationDTO  : PaginationDto
    {
        /// <summary>
        /// A text for filtering results.
        /// </summary>
        public string? Text { get; set; }
    }
}