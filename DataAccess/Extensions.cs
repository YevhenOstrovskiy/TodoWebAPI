using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataAccess
{
    public static class Extensions
    {
        public static IServiceCollection AddData(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddScoped<ITodoRepository, TodoRepository>();
            serviceCollection.AddDbContext<TodoDb>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            return serviceCollection;

        }
    }
}
