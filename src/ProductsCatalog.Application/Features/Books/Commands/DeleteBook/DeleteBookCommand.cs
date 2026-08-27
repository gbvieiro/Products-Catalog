using MediatR;
using ProductsCatalog.Application.Common.Messaging;

namespace ProductsCatalog.Application.Features.Books.Commands.DeleteBook;

public sealed record DeleteBookCommand(Guid Id) : ICommand<Unit>;
