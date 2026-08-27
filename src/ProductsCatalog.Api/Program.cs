using Microsoft.OpenApi.Models;
using ProductsCatalog.Api.Middleware;
using ProductsCatalog.Application;
using ProductsCatalog.Infrastructure;

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
});

var app = builder.Build();

// Precisa vir antes de tudo: converte qualquer excecao das camadas internas em uma resposta HTTP consistente.
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Products Catalog v1");
});

app.UseHttpsRedirection();
app.UseCors(cors => cors.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

// NOTA: nenhum esquema de autenticacao (AddAuthentication/AddJwtBearer) esta
// configurado ainda - isso ja vinha assim no projeto original (o pacote
// JwtBearer estava referenciado mas nunca usado). Login/JWT ficam fora do
// escopo desta refatoracao (focada em Clean Architecture + CQRS).
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

// Necessario para WebApplicationFactory<Program> nos testes de integracao.
public partial class Program
{
}
