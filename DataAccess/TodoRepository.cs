using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    internal class TodoRepository(TodoDb context) : ITodoRepository
    {

        public async Task<List<Todo>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await context.Todos.ToListAsync(cancellationToken);
        }
        
        public async Task<List<Todo>> GetCompletedAsync(CancellationToken cancellationToken = default)
        {
            return await context.Todos.Where(x => x.IsComplete).ToListAsync();
        }

        public async Task<List<Todo>> GetUncompletedAsync(CancellationToken cancellationToken = default)
        {
            return await context.Todos.Where(x => !x.IsComplete).ToListAsync();
        }

        public async Task<Todo?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await context.Todos.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Todo>?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await context.Todos
            .Where(t => t.Name != null && t.Name.ToLower().Contains(name.ToLower()))
            .ToListAsync(cancellationToken);
        }

        public async Task CreateAsync(Todo todo, CancellationToken cancellationToken = default)
        {
            await context.Todos.AddAsync(todo, cancellationToken);
            await context.SaveChangesAsync();
        }

        public async Task CreateBulkAsync(List<Todo> newTodos, CancellationToken cancellationToken = default)
        {
            await context.Todos.AddRangeAsync(newTodos, cancellationToken);
            await context.SaveChangesAsync();
        }

        public async Task UpdateByIdAsync(Todo todo, CancellationToken cancellationToken = default)
        {
            context.Todos.Update(todo);
            await context.SaveChangesAsync(cancellationToken);

        }

        public async Task DeleteAsync(Todo todo, CancellationToken cancellationToken = default)
        {
            context.Todos.Remove(todo);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteBulkAsync(List<Todo> todos, CancellationToken cancellationToken = default)
        {
            context.Todos.RemoveRange(todos);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
