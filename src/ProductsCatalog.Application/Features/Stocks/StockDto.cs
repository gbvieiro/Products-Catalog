using ProductsCatalog.Application.Common.Mappings;
using ProductsCatalog.Application.Features.Books;
using ProductsCatalog.Domain.Entities;

namespace ProductsCatalog.Application.Features.Stocks;

public sealed record StockDto : IMapFrom<Stock>
{
    public Guid Id { get; init; }
    public Guid BookId { get; init; }
    public int Quantity { get; init; }
}

/// <summary>StockDto "rico", com os dados do livro embutidos (usado por GetStockByBookId).</summary>
public sealed record CompleteStockDto
{
    public Guid Id { get; init; }
    public Guid BookId { get; init; }
    public int Quantity { get; init; }
    public required BookDto Book { get; init; }
}
