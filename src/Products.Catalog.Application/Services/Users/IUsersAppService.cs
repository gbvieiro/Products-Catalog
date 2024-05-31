using Products.Catalog.Application.DTOs;
using Products.Catalog.Application.Services.Common;

namespace Products.Catalog.Application.Services.Users
{
    /// <summary>
    /// Provide a interface for user domain user cases.
    /// </summary>
    public interface IUsersAppService : ICrudAppService<UserDto>
    {
    }
}