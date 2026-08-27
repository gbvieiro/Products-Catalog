using MediatR;
using ProductsCatalog.Application.Common.Messaging;
using ProductsCatalog.Domain.Enums;

namespace ProductsCatalog.Application.Features.Users.Commands.UpdateUser;

public sealed record UpdateUserCommand(Guid Id, string Email, ERole Role) : ICommand<Unit>;
