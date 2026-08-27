using AutoMapper;
using MediatR;
using ProductsCatalog.Domain.Repositories;

namespace ProductsCatalog.Application.Features.Users.Queries.GetUserById;

public sealed class GetUserByIdQueryHandler(IUserRepository userRepository, IMapper mapper)
    : IRequestHandler<GetUserByIdQuery, UserDto?>
{
    public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.Id, cancellationToken);
        return user is null ? null : mapper.Map<UserDto>(user);
    }
}
