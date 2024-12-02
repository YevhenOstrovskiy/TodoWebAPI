using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace DataAccess
{
    public static class Extensions
    {
        public static IServiceCollection AddData(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<ITodoRepository, TodoRepository>();
            serviceCollection.AddDbContext<TodoDb>(options =>
            {
                options.UseSqlServer("Server=DESKTOP-U1B8C0E\\MSSQLSERVER01;Database=TodoDb;Trusted_Connection=True;TrustServerCertificate=True;");
            });

            return serviceCollection;

        }
    }
}
