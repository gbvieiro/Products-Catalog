
using Microsoft.IdentityModel.Tokens;
using Products.Catalog.Domain.RepositoriesInterfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Products.Catalog.Infra.Authentication
{
    /// <summary>
    /// The authentication service.
    /// </summary>
    /// <param name="usersRepository">A user repository.</param>
    public class AuthenticationService(IUsersRepository usersRepository) : IAuthenticationService
    {
        private readonly IUsersRepository _usersRepository = usersRepository;

        /// <inheritdoc/>
        public async Task<dynamic?> AuthenticateUser(AuthenticationModel userDto)
        {
            var  user = await _usersRepository.GetByEmailAsync(userDto.Email);

            if (user == null)
            {
                return null;
            }

            if (user.Password == userDto.Password) 
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                var key = Encoding.ASCII.GetBytes(AuthenticationConfigs._secretKey);

                var tokenDescription = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.Email.ToString()),
                        new Claim(ClaimTypes.Role, user.Role.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddHours(2),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescription));
                return new { Token = token };
            } 
            else
            {
                return null;
            }
        }
    }
}