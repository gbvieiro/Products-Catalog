using MediatR;
using ProductsCatalog.Application.Common.Exceptions;
using ProductsCatalog.Domain.Entities;
using ProductsCatalog.Domain.Repositories;
using ProductsCatalog.Domain.ValueObjects;

namespace ProductsCatalog.Application.Features.Users.Commands.UpdateUser;

public sealed class UpdateUserCommandHandler(IUserRepository userRepository) : IRequestHandler<UpdateUserCommand, Unit>
{
    public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(User), request.Id);

        user.Update(new Email(request.Email), request.Role);
        userRepository.Update(user);

        return Unit.Value;
    }
}
