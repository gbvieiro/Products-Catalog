using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Products.Catalog.Infra.Authentication
{
    public static class AuthenticationConfigs
    {
        public static readonly string _secretKey = "6cd3556deb0da54bca060b4c39479839e4099838f212f115f27c0a4c21b8d7f6";

        public const string Admin = "admin";
        public const string Seller = "seller";
        public const string Client = "client";

        public static void AddJWTAuthentication(this IServiceCollection services)
        {
            var key = Encoding.ASCII.GetBytes(_secretKey);
            services.AddAuthentication(s =>
            {
                s.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                s.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(s =>
            {
                s.RequireHttpsMetadata = false;
                s.SaveToken = true;
                s.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });
        }

        public static void AddSwaggerWithAuthorizeButton(
            this IServiceCollection services,
            string projectTitle,
            string description,
            string version,
            string developerName,
            string developerEmail,
            string developerURI,
            string schemeName,
            string headerName
        )
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(version, new OpenApiInfo
                {
                    Title = projectTitle,
                    Version = version,
                    Description = description,
                    Contact = new OpenApiContact()
                    {
                        Name = developerName,
                        Email = developerEmail,
                        Url = new Uri(developerURI)
                    }
                }); 

                var scheme = new OpenApiSecurityScheme()
                {
                    Name = headerName,
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                    Scheme = schemeName,
                    BearerFormat = "String"
                };

                var reference = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = schemeName
                    }
                };

                c.AddSecurityDefinition(schemeName, scheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement { { reference, Array.Empty<string>() } });
            });
        }
    }
}