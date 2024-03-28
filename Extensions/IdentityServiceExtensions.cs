using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace API.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) // add an authentication service to the app and use JWT authentication
                .AddJwtBearer(options =>   // Configure JWT authentication options
                {
                    options.TokenValidationParameters = new TokenValidationParameters  // Configure JWT authentication parameters
                    {
                        ValidateIssuerSigningKey = true,  // Verify the signing key of the token
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding  // The secret key is used to sign and decrypt the token
                                            .UTF8.GetBytes(config["TokenKey"])), 
                        ValidateIssuer = false, // donot validate issuer
                        ValidateAudience = false // donot validate audience
                    };
                });
            return services;
        }
    }
}
