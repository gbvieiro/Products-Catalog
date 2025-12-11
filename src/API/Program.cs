using Microsoft.OpenApi.Models;
using Products.Catalog.Application;
using Products.Catalog.Application.AutoMapper;
using Product.Catalog.Infra.IOC;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterApplication();
builder.Services.RegisterInfra(builder.Configuration);
builder.Services.AddMapperService();
builder.Services.AddCors();

builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Books Catalog",
        Version = "v1",
        Description = "This is a book catalog.",
        Contact = new OpenApiContact()
        {
            Name = "Gabriel Menegazzi Vieiro",
            Email = "gbvieiro@gmail.com",
            Url = new Uri("https://www.linkedin.com/in/gbvieiro/")
        }
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Books Catalog v1");
});
app.UseHttpsRedirection();
app.UseCors(cors => { cors.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();