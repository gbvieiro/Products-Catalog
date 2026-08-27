using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using ProductsCatalog.Application.Common.Interfaces;
using ProductsCatalog.Domain.Enums;

namespace ProductsCatalog.Infrastructure.Services;

/// <summary>
/// Emite tokens JWT (HS256) assinados na mao, sem depender de pacotes
/// externos - mesmo espirito do PasswordHasher (PBKDF2 via
/// Rfc2898DeriveBytes). So o minimo necessario para EMITIR o token:
/// header + payload + assinatura em Base64Url. A VALIDACAO na Api usa o
/// pacote Microsoft.AspNetCore.Authentication.JwtBearer normalmente (ver
/// Program.cs) - so a emissao (lado do servidor de login) e feita na mao
/// aqui.
///
/// Os claims de identidade (NameIdentifier/Email/Role) usam os mesmos
/// nomes de ClaimTypes.* que o ASP.NET Core espera por padrao ao validar
/// (TokenValidationParameters.DefaultRoleClaimType == ClaimTypes.Role,
/// por exemplo), entao [Authorize(Roles = "Administrator")] funciona sem
/// nenhuma configuracao adicional de mapeamento de claims.
/// </summary>
public class JwtTokenGenerator(IConfiguration configuration) : IJwtTokenGenerator
{
    public JwtToken Generate(Guid userId, string email, ERole role)
    {
        var secret = configuration["Jwt:Secret"]
            ?? throw new InvalidOperationException("Jwt:Secret nao esta configurado.");
        var issuer = configuration["Jwt:Issuer"] ?? "ProductsCatalog";
        var audience = configuration["Jwt:Audience"] ?? "ProductsCatalog";
        // Leitura manual (em vez de IConfiguration.GetValue<T>()) de proposito:
        // esse metodo de extensao vive no pacote Microsoft.Extensions.Configuration.Binder,
        // que este projeto nao referencia - e evitamos adicionar um pacote novo so
        // por isso (mesmo espirito de nao depender de pacotes externos alem do
        // estritamente necessario, ver PasswordHasher/JwtTokenGenerator).
        var expirationMinutes = int.TryParse(configuration["Jwt:ExpirationMinutes"], out var configuredMinutes)
            ? configuredMinutes
            : 480;

        var issuedAt = DateTimeOffset.UtcNow;
        var expiresAt = issuedAt.AddMinutes(expirationMinutes);

        var header = new Dictionary<string, object>
        {
            ["alg"] = "HS256",
            ["typ"] = "JWT",
        };

        var payload = new Dictionary<string, object>
        {
            ["sub"] = userId.ToString(),
            [ClaimTypes.NameIdentifier] = userId.ToString(),
            [ClaimTypes.Email] = email,
            [ClaimTypes.Role] = role.ToString(),
            ["iss"] = issuer,
            ["aud"] = audience,
            ["iat"] = issuedAt.ToUnixTimeSeconds(),
            ["exp"] = expiresAt.ToUnixTimeSeconds(),
            ["jti"] = Guid.NewGuid().ToString(),
        };

        var headerSegment = Base64UrlEncode(JsonSerializer.SerializeToUtf8Bytes(header));
        var payloadSegment = Base64UrlEncode(JsonSerializer.SerializeToUtf8Bytes(payload));
        var unsignedToken = $"{headerSegment}.{payloadSegment}";

        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
        var signature = Base64UrlEncode(hmac.ComputeHash(Encoding.UTF8.GetBytes(unsignedToken)));

        return new JwtToken($"{unsignedToken}.{signature}", expiresAt.UtcDateTime);
    }

    private static string Base64UrlEncode(byte[] input) =>
        Convert.ToBase64String(input).TrimEnd('=').Replace('+', '-').Replace('/', '_');
}
