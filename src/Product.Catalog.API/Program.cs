using Product.Catalog.Infra.IOC;
using Products.Catalog.Infra.Authentication;
using Products.Catalog.Infra.Mapper;

// Create builder.
var builder = WebApplication.CreateBuilder(args);

// Configure services
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

// Build app.
var app = builder.Build();

// Configure app.
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseCors(cors => { cors.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();