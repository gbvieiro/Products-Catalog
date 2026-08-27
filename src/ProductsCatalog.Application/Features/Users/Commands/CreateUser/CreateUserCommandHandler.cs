using MediatR;
using ProductsCatalog.Application.Common.Interfaces;
using ProductsCatalog.Domain.Entities;
using ProductsCatalog.Domain.Repositories;
using ProductsCatalog.Domain.ValueObjects;

namespace ProductsCatalog.Application.Features.Users.Commands.CreateUser;

/// <summary>A senha em texto puro nunca chega a entrar no dominio: e sempre hasheada antes.</summary>
public sealed class CreateUserCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher) : IRequestHandler<CreateUserCommand, Guid>
{
    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var passwordHash = passwordHasher.Hash(request.Password);
        var user = new User(new Email(request.Email), passwordHash, request.Role);

        await userRepository.AddAsync(user, cancellationToken);

        return user.Id;
    }
}
