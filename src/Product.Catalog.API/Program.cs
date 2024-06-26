using Product.Catalog.Infra.IOC;
using Products.Catalog.Infra.Authentication;
using Products.Catalog.Infra.Mapper;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure();
builder.Services.AddMapperService();
builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerWithAuthorizeButton(
    "Books Catalog",
    "This is a book catalog.",
    "v1",
    "Gabriel Menegazzi Vieiro",
    "'gbvieiro@gmail.com",
    "https://www.linkedin.com/in/gbvieiro/",
    "ProductCatalogToken",
    "Authorization"
);
builder.Services.AddJWTAuthentication();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseCors(cors => { cors.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();