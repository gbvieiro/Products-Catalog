using System.Net.Http.Headers;
using System.Net.Http.Json;
using ProductsCatalog.Application.Features.Auth;
using ProductsCatalog.Application.Features.Auth.Commands.Login;
using ProductsCatalog.Infrastructure.Persistence;

namespace ProductsCatalog.Api.IntegrationTests.Common;

/// <summary>
/// Helper compartilhado pelos testes de integracao: cria um HttpClient ja
/// autenticado (header Authorization: Bearer preenchido), logando como o
/// usuario administrador padrao. Esse usuario e seedado automaticamente por
/// ApplicationDbContextSeeder sempre que a Api sobe em Development - o que
/// inclui toda instancia de ApiWebApplicationFactory, ja que
/// WebApplicationFactory usa "Development" como ambiente por padrao (ver
/// comentario em ApiWebApplicationFactory sobre o schema ser criado do mesmo
/// jeito). Sem isso, cada teste teria que duplicar o fluxo de login.
///
/// A maioria dos testes de integracao so precisa de QUALQUER usuario
/// autenticado (a policy padrao e "usuario autenticado"; restricoes de role
/// especificas ja sao cobertas separadamente em AuthEndpointsTests). Por
/// isso usar sempre o admin aqui e suficiente e mantem os testes de
/// Books/Orders/etc focados no comportamento que eles realmente testam.
/// </summary>
public static class ApiWebApplicationFactoryExtensions
{
    public static async Task<HttpClient> CreateAuthenticatedClientAsync(this ApiWebApplicationFactory factory)
    {
        var client = factory.CreateClient();

        var loginResponse = await client.PostAsJsonAsync("/api/auth/login", new LoginCommand(
            ApplicationDbContextSeeder.DefaultAdminEmail,
            ApplicationDbContextSeeder.DefaultAdminPassword));
        loginResponse.EnsureSuccessStatusCode();

        var result = await loginResponse.Content.ReadFromJsonAsync<LoginResult>();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result!.Token);

        return client;
    }
}
