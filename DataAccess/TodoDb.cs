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
        public DbSet<Account> Accounts { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Todo>()
                .HasOne(todo => todo.Account)
                .WithMany(account => account.Todos)
                .HasForeignKey(todo => todo.AccountId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
