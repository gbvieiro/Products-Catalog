using ProductsCatalog.Domain.Enums;

namespace ProductsCatalog.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    JwtToken Generate(Guid userId, string email, ERole role);
}

public sealed record JwtToken(string Value, DateTime ExpiresAtUtc);
