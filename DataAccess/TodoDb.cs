using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class TodoDb : DbContext
    {
        public TodoDb(DbContextOptions<TodoDb> options) 
            : base(options) 
        {
            Database.EnsureCreated();
        }

        public DbSet<Todo> Todos { get; set; } = null!;
    }
}
