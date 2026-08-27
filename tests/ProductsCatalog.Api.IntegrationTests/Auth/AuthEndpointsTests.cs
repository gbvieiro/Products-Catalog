using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using ProductsCatalog.Api.IntegrationTests.Common;
using ProductsCatalog.Application.Features.Auth;
using ProductsCatalog.Application.Features.Auth.Commands.Login;
using ProductsCatalog.Domain.Enums;
using ProductsCatalog.Infrastructure.Persistence;
using Xunit;

namespace ProductsCatalog.Api.IntegrationTests.Auth;

/// <summary>
/// Cobre o fluxo de login/logout e a protecao basica dos endpoints (usuario
/// nao autenticado deve levar 401). O usuario administrador padrao usado
/// aqui e criado automaticamente por ApplicationDbContextSeeder toda vez que
/// a Api sobe em Development - o que inclui esta factory (ver comentario em
/// ApiWebApplicationFactoryExtensions).
/// </summary>
public class AuthEndpointsTests(ApiWebApplicationFactory factory) : IClassFixture<ApiWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Login_WithSeededAdminCredentials_ReturnsTokenAndUserInfo()
    {
        var response = await _client.PostAsJsonAsync("/api/auth/login", new LoginCommand(
            ApplicationDbContextSeeder.DefaultAdminEmail,
            ApplicationDbContextSeeder.DefaultAdminPassword));

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<LoginResult>();

        result.Should().NotBeNull();
        result!.Token.Should().NotBeNullOrWhiteSpace();
        result.User.Email.Should().Be(ApplicationDbContextSeeder.DefaultAdminEmail);
        result.User.Role.Should().Be(ERole.Administrator);
    }

    [Fact]
    public async Task Login_WithWrongPassword_ReturnsUnauthorized()
    {
        var response = await _client.PostAsJsonAsync("/api/auth/login", new LoginCommand(
            ApplicationDbContextSeeder.DefaultAdminEmail, "not-the-right-password"));

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Login_WithUnknownEmail_ReturnsUnauthorized()
    {
        var response = await _client.PostAsJsonAsync("/api/auth/login", new LoginCommand(
            "nobody@example.com", "whatever"));

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task ProtectedEndpoint_WithoutToken_ReturnsUnauthorized()
    {
        // GET /api/users exige qualquer usuario autenticado (fallback policy) -
        // sem [AllowAnonymous]/Authorization header, deve ser barrado.
        var response = await _client.GetAsync("/api/users");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task ProtectedEndpoint_WithToken_Succeeds()
    {
        var authenticatedClient = await factory.CreateAuthenticatedClientAsync();

        var response = await authenticatedClient.GetAsync("/api/users");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Logout_WithToken_ReturnsNoContent()
    {
        var authenticatedClient = await factory.CreateAuthenticatedClientAsync();

        var response = await authenticatedClient.PostAsync("/api/auth/logout", null);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
