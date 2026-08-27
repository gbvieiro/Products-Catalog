using ProductsCatalog.Application.Common.Messaging;

namespace ProductsCatalog.Application.Features.Auth.Commands.Login;

public sealed record LoginCommand(string Email, string Password) : ICommand<LoginResult>;
