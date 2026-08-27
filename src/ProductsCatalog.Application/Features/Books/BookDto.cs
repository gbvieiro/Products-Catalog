using ProductsCatalog.Application.Common.Mappings;
using ProductsCatalog.Domain.Entities;
using ProductsCatalog.Domain.Enums;

namespace ProductsCatalog.Application.Features.Books;

public sealed record BookDto : IMapFrom<Book>
{
    public Guid Id { get; init; }
    public double Price { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Author { get; init; } = string.Empty;
    public EBookGenre Genre { get; init; }
    public DateTime CreatedAt { get; init; }
}
