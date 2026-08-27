using ProductsCatalog.Application.Common.Messaging;
using ProductsCatalog.Domain.Enums;

namespace ProductsCatalog.Application.Features.Users.Commands.CreateUser;

public sealed record CreateUserCommand(string Email, string Password, ERole Role) : ICommand<Guid>;
