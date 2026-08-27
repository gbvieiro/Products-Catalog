using Microsoft.EntityFrameworkCore;
using ProductsCatalog.Application.Common.Interfaces;
using ProductsCatalog.Domain.Entities;
using ProductsCatalog.Domain.Enums;
using ProductsCatalog.Domain.ValueObjects;

namespace ProductsCatalog.Infrastructure.Persistence;

/// <summary>
/// Cria o usuario administrador padrao (admin@email.com / admin) na primeira
/// vez que a Api sobe, para permitir logar e cadastrar os demais usuarios -
/// sem isso, ninguem conseguiria logar num banco recem-criado. Chamado a
/// partir de Program.cs, no mesmo bloco de Development que ja cria o schema
/// via EnsureCreated() (ver Persistence/Migrations/README.md).
///
/// IMPORTANTE: troque essa senha assim que possivel - ela existe so para
/// bootstrap do primeiro acesso.
/// </summary>
public static class ApplicationDbContextSeeder
{
    public const string DefaultAdminEmail = "admin@email.com";
    public const string DefaultAdminPassword = "admin";

    public static async Task SeedAsync(ApplicationDbContext context, IPasswordHasher passwordHasher)
    {
        var alreadySeeded = await context.Users.AnyAsync(u => u.Email.Address == DefaultAdminEmail);
        if (alreadySeeded)
        {
            return;
        }

        var admin = new User(
            new Email(DefaultAdminEmail),
            passwordHasher.Hash(DefaultAdminPassword),
            ERole.Administrator);

        context.Users.Add(admin);

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            // Corrida entre instancias concorrentes subindo a Api ao mesmo tempo
            // contra o mesmo Postgres (ex: varias classes de teste de integracao
            // em paralelo - ver SchemaCreationLock em ApiWebApplicationFactory).
            // Email e unique: se outra instancia ja inseriu o mesmo admin entre
            // o AnyAsync acima e este SaveChangesAsync, nao ha nada a fazer.
        }
    }
}
