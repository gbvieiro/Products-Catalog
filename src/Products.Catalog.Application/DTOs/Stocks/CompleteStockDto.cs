namespace Products.Catalog.Application.DTOs.Stocks
{
    /// <summary>
    /// Complete stock structure that contains the book registration.
    /// </summary>
    public class CompleteStockDto : StockDto
    {
        /// <summary>
        /// Book information.
        /// </summary>
        public required BookDto Book { get; set; }
    }
}