using MediatR;
using ProductsCatalog.Application.Common.Exceptions;
using ProductsCatalog.Application.Common.Interfaces;
using ProductsCatalog.Domain.Repositories;

namespace ProductsCatalog.Application.Features.Auth.Commands.Login;

public sealed class LoginCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator) : IRequestHandler<LoginCommand, LoginResult>
{
    public async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(request.Email, cancellationToken);

        // Mensagem generica de proposito: nao da pra um atacante distinguir
        // "email nao existe" de "senha errada".
        if (user is null || !passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedException("Invalid email or password.");
        }

        var token = jwtTokenGenerator.Generate(user.Id, user.Email.Address, user.Role);

        return new LoginResult
        {
            Token = token.Value,
            ExpiresAtUtc = token.ExpiresAtUtc,
            User = new AuthenticatedUserDto { Id = user.Id, Email = user.Email.Address, Role = user.Role },
        };
    }
}
