using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProductsCatalog.Api.Middleware;
using ProductsCatalog.Application;
using ProductsCatalog.Application.Common.Interfaces;
using ProductsCatalog.Infrastructure;
using ProductsCatalog.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Composition root: cada camada expoe seu proprio AddXxx(), Program.cs so os conecta.
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Products Catalog",
        Version = "v1",
        Description = "Catalogo de livros, pedidos e estoque - exemplo de Clean Architecture + CQRS/MediatR.",
        Contact = new OpenApiContact
        {
            Name = "Gabriel Menegazzi Vieiro",
            Email = "gbvieiro@gmail.com",
            Url = new Uri("https://www.linkedin.com/in/gbvieiro/")
        }
    });

    // Permite testar endpoints autenticados direto pelo Swagger UI: cole o
    // token retornado por POST /api/auth/login (sem o prefixo "Bearer ").
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Informe apenas o token JWT (sem o prefixo 'Bearer '); o Swagger adiciona o prefixo automaticamente.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

// Jwt:Secret e obrigatorio - sem ele nao ha como assinar/validar tokens.
// So existe um valor (dev-only, nao usar em producao) em
// appsettings.Development.json; fora de Development, a ausencia derruba o
// startup imediatamente em vez de silenciosamente aceitar tokens invalidos.
var jwtSecret = builder.Configuration["Jwt:Secret"]
    ?? throw new InvalidOperationException(
        "Configuracao 'Jwt:Secret' ausente. Defina-a (appsettings.Development.json ja tem um valor de desenvolvimento; " +
        "em outros ambientes, configure via variavel de ambiente Jwt__Secret ou um cofre de segredos real).");
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "ProductsCatalog";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "ProductsCatalog";

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,
            ValidateAudience = true,
            ValidAudience = jwtAudience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(1)
        };
    });

// Politica padrao: qualquer endpoint sem [AllowAnonymous]/[Authorize] explicito
// exige um usuario autenticado. Restricoes por role sao aplicadas endpoint a
// endpoint com [Authorize(Roles = nameof(ERole.Administrator))] (ver
// controllers) - Administrator pode tudo, Seller so cria/ve pedidos.
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Ainda nao existem migrations geradas para este modelo (ver
    // src/ProductsCatalog.Infrastructure/Persistence/Migrations/README.md).
    // Para poder subir o backend localmente (ex: via docker-compose, contra
    // um Postgres real) sem precisar gerar/aplicar uma migration na mao
    // primeiro, criamos o schema direto a partir do modelo atual - mesma
    // estrategia usada pelos testes de integracao (ver ApiWebApplicationFactory).
    // EnsureCreated() e idempotente: reinicios subsequentes so confirmam que
    // o schema ja existe. Assim que a primeira migration for gerada, troque
    // isto por context.Database.Migrate().
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();

    // Cria o usuario administrador padrao (admin@email.com / admin) se ainda
    // nao existir, para permitir o primeiro login e o cadastro dos demais
    // usuarios a partir dele. Ver ApplicationDbContextSeeder para detalhes.
    var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
    await ApplicationDbContextSeeder.SeedAsync(context, passwordHasher);
}

// Precisa vir antes de tudo: converte qualquer excecao das camadas internas em uma resposta HTTP consistente.
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Products Catalog v1");
});

app.UseHttpsRedirection();
app.UseCors(cors => cors.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

// Precisa vir nesta ordem (Authentication antes de Authorization) e depois
// de UseCors/UseHttpsRedirection: valida o JWT (se presente) e popula
// User/HttpContext.User, para so entao a policy de autorizacao (fallback =
// usuario autenticado, mais os [Authorize(Roles=...)] dos controllers) ser avaliada.
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

// Necessario para WebApplicationFactory<Program> nos testes de integracao.
public partial class Program
{
}
