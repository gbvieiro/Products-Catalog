using MediatR;
using ProductsCatalog.Application.Common.Messaging;
using ProductsCatalog.Domain.Enums;

namespace ProductsCatalog.Application.Features.Books.Commands.UpdateBook;

public sealed record UpdateBookCommand(Guid Id, double Price, string Title, string Author, EBookGenre Genre)
    : ICommand<Unit>;
