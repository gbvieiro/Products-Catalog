using ProductsCatalog.Application.Common.Messaging;

namespace ProductsCatalog.Application.Features.Users.Queries.GetUserById;

public sealed record GetUserByIdQuery(Guid Id) : IQuery<UserDto?>;
