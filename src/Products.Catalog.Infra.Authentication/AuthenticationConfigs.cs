using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Products.Catalog.Infra.Authentication
{
    /// <summary>
    /// Use this class to add authentication configs for this project.
    /// </summary>
    public static class AuthenticationConfigs
    {
        /// <summary>
        /// This is a secret key that is used to generate the JWT token.
        /// </summary>
        public static readonly string _secretKey = "6cd3556deb0da54bca060b4c39479839e4099838f212f115f27c0a4c21b8d7f6";

        // Define names in a static field avoid problems to rename it when necessary.
        public static readonly string AdminRuleName = "admin";
        public static readonly string SellerRuleName = "seller";
        public static readonly string ClientsRuleName = "client";

        /// <summary>
        /// Add JWT authetication to a service collection.
        /// </summary>
        /// <param name="services">Current project service collection.</param>
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


        /// <summary>
        /// Add swagger but with a authorize button.
        /// </summary>
        /// <param name="services">Current project service collection.</param>
        /// <param name="projectTitle">A project title.</param>
        /// <param name="description">A project description.</param>
        /// <param name="version">A project version.</param>
        /// <param name="developerName">A developer name.</param>
        /// <param name="developerEmail">A developer email.</param>
        /// <param name="developerURI">A developer URI.</param>
        /// <param name="schemeName">A scheme name. (Must be unique)</param>
        /// <param name="headerName">A header name to receive a token.</param>
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