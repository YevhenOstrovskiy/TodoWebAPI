using Microsoft.Extensions.DependencyInjection;

namespace Businesslogic
{
    public static class Extensions
    {
        public static IServiceCollection AddBusinessLogic(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<ITodoService, TodoService>();
            return serviceCollection;
        }
    }
}
