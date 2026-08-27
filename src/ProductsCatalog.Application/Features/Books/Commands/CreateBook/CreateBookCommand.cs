using ProductsCatalog.Application.Common.Messaging;
using ProductsCatalog.Domain.Enums;

namespace ProductsCatalog.Application.Features.Books.Commands.CreateBook;

public sealed record CreateBookCommand(double Price, string Title, string Author, EBookGenre Genre) : ICommand<Guid>;
