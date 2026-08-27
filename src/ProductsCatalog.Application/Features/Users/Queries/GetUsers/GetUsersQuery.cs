using ProductsCatalog.Application.Common.Messaging;
using ProductsCatalog.Application.Common.Models;

namespace ProductsCatalog.Application.Features.Users.Queries.GetUsers;

public sealed record GetUsersQuery(string? Filter = null, int Skip = 0, int Take = 20) : IQuery<PagedResult<UserDto>>;
