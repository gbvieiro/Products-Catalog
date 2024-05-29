namespace Products.Catalog.Application.DTOs.Pagination
{
    /// <summary>
    /// A pagination model.
    /// </summary>
    public class PaginationDto
    {
        /// <summary>
        /// Number of itens to skip.
        /// </summary>
        public int Skip { get; set; }

        /// <summary>
        /// Number of itens to take.
        /// </summary>
        public int Take { get; set; }
    }
}