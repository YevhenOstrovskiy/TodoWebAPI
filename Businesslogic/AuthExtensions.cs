using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public static class AuthExtensions
    {
        public static IServiceCollection AddAuth(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.Configure<AuthSettings>(configuration.GetSection("AuthSettings"));
            serviceCollection.AddScoped<IAccountService, AccountService>();
            serviceCollection.AddScoped<JwtService>();
            var authSettings = configuration.GetSection(nameof(AuthSettings))
                .Get<AuthSettings>();
            serviceCollection.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.SecretKey))
                    };
                });
            return serviceCollection;
        }
    }
}
