using ProductsCatalog.Application.Common.Messaging;

namespace ProductsCatalog.Application.Features.Users.Commands.CreateUser;

public sealed record CreateUserCommand(string Email, string Password, string Role) : ICommand<Guid>;
