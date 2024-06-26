using Products.Catalog.Application.DTOs;
using Products.Catalog.Application.Services.Common;

namespace Products.Catalog.Application.Services.Users
{
    public interface IUsersAppService : ICrudAppService<UserDto> { }
}