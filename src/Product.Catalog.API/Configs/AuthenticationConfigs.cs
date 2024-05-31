using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Product.Catalog.API.Configs
{
    /// <summary>
    /// Use this class to add authentication configs for this project.
    /// </summary>
    public static class AuthenticationConfigs
    {
        /// <summary>
        /// This is a secret key that is used to generate the JWT token.
        /// </summary>
        private static readonly string _secretKey = "6cd3556deb0da54bca060b4c39479839e4099838f212f115f27c0a4c21b8d7f6";

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
    }
}