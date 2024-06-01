using Product.Catalog.Infra.IOC;
using Products.Catalog.Infra.Authentication;
using Products.Catalog.Infra.Mapper;

var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services.AddInfrastructure();
builder.Services.AddMapperService();
builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerWithAuthorizeButton("ProductCatalogToken", "Authorization");
builder.Services.AddJWTAuthentication();

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