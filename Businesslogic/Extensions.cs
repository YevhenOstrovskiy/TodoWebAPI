using BusinessLogic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Businesslogic
{
    public static class Extensions
    {
        public static IServiceCollection AddBusinessLogic(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.Configure<AuthSettings>(configuration.GetSection("AuthSettings"));
            serviceCollection.AddScoped<ITodoService, TodoService>();
            serviceCollection.AddScoped<IAccountService, AccountService>();
            serviceCollection.AddScoped<JwtService>();
            return serviceCollection;
        }
    }
}
