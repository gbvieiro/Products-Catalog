
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
        public async Task<string> GenerateToken(AuthenticationModel userDto)
        {
            var  user = await _usersRepository.GetByEmailAsync(userDto.Email);

            if (user == null)
                return string.Empty;

            if (user.Password == userDto.Password)
                return GenerateToken(user.Id.ToString(), userDto.Email, user.Role.ToLower());
            
            return string.Empty;
        }

        /// <summary>
        /// Generates a new token.
        /// </summary>
        /// <param name="userId">A user id.</param>
        /// <param name="userName">A user name.</param>
        /// <param name="role">A user role.</param>
        /// <returns>A token.</returns>
        private static string GenerateToken(string userId, string userName, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(AuthenticationConfigs._secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new(ClaimTypes.NameIdentifier, userId),
                    new(ClaimTypes.Name, userName),
                    new(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}