using MediatR;
using ProductsCatalog.Application.Common.Exceptions;
using ProductsCatalog.Domain.Entities;
using ProductsCatalog.Domain.Repositories;

namespace ProductsCatalog.Application.Features.Users.Commands.DeleteUser;

public sealed class DeleteUserCommandHandler(IUserRepository userRepository) : IRequestHandler<DeleteUserCommand, Unit>
{
    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(User), request.Id);

        userRepository.Remove(user);

        return Unit.Value;
    }
}
