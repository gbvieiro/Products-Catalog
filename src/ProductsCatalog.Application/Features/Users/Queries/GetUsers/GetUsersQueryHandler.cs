using AutoMapper;
using MediatR;
using ProductsCatalog.Application.Common.Models;
using ProductsCatalog.Domain.Repositories;
using ProductsCatalog.Domain.Specifications;

namespace ProductsCatalog.Application.Features.Users.Queries.GetUsers;

public sealed class GetUsersQueryHandler(IUserRepository userRepository, IMapper mapper)
    : IRequestHandler<GetUsersQuery, PagedResult<UserDto>>
{
    public async Task<PagedResult<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var spec = new UsersFilterSpecification(request.Filter, request.Skip, request.Take);

        var users = await userRepository.ListAsync(spec, cancellationToken);
        var total = await userRepository.CountAsync(spec, cancellationToken);

        return new PagedResult<UserDto>(mapper.Map<List<UserDto>>(users), total, request.Skip, request.Take);
    }
}
