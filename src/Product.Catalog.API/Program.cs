using Product.Catalog.Infra.IOC;
using Products.Catalog.Infra.Authentication;
using Products.Catalog.Infra.Mapper;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterApplication();
builder.Services.RegisterAuthentication();
builder.Services.RegisterInfra(builder.Configuration);
builder.Services.AddMapperService();
builder.Services.AddCors();

builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerWithAuthorizeButton(
    "Books Catalog",
    "This is a book catalog.",
    "v1",
    "Gabriel Menegazzi Vieiro",
    "gbvieiro@gmail.com",
    "https://www.linkedin.com/in/gbvieiro/",
    "ProductCatalogToken",
    "Authorization"
);
builder.Services.AddJWTAuthentication();

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