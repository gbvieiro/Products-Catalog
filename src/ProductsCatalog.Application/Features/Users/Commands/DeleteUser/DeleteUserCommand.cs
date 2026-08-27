using MediatR;
using ProductsCatalog.Application.Common.Messaging;

namespace ProductsCatalog.Application.Features.Users.Commands.DeleteUser;

public sealed record DeleteUserCommand(Guid Id) : ICommand<Unit>;
