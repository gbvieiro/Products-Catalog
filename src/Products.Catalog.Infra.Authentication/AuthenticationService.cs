
using Microsoft.IdentityModel.Tokens;
using Products.Catalog.Domain.Entities;
using Products.Catalog.Domain.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Products.Catalog.Infra.Authentication
{
    public class AuthenticationService(IRepository<User> usersRepository) : IAuthenticationService
    {
        private readonly IRepository<User> _usersRepository = usersRepository;

        public async Task<string> GenerateToken(AuthenticationModel userDto)
        {
            var allUsers = await _usersRepository.FindAsync(string.Empty, 0, int.MaxValue);
            var user = allUsers.FirstOrDefault(u => u.Email.Address == userDto.Email);

            if (user == null)
                return string.Empty;

            if (user.Password == userDto.Password)
                return GenerateToken(user.Id.ToString(), userDto.Email, user.Role.ToLower());
            
            return string.Empty;
        }

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