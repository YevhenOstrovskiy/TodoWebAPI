using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    internal class TodoRepository(TodoDb context) : ITodoRepository
    {

        public async Task<List<Todo>> GetAllAsync(Guid? accountId, CancellationToken cancellationToken = default)
        {
            return await context.Todos.Where(x => x.AccountId == accountId).ToListAsync();
        }
        
        public async Task<List<Todo>> GetCompletedAsync(Guid? accountId, CancellationToken cancellationToken = default)
        {
            return await context.Todos.Where(x => x.AccountId == accountId && x.IsComplete).ToListAsync();
        }

        public async Task<List<Todo>> GetUncompletedAsync(Guid? accountId, CancellationToken cancellationToken = default)
        {
            return await context.Todos.Where(x => x.AccountId == accountId && !x.IsComplete).ToListAsync();
        }

        public async Task<Todo?> GetByIdAsync(Guid? accountId, Guid id, CancellationToken cancellationToken = default)
        {
            return await context.Todos.FirstOrDefaultAsync(x => x.AccountId == accountId && x.Id == id);
        }

        public async Task<List<Todo>?> GetByNameAsync(Guid? accountId, string name, CancellationToken cancellationToken = default)
        {
            return await context.Todos
            .Where(x => x.AccountId == accountId && x.Name != null && x.Name.ToLower().Contains(name.ToLower()))
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

        public async Task UpdateByIdAsync(Guid? accountId, Todo todo, CancellationToken cancellationToken = default)
        {
            if (todo.AccountId == accountId)
            {
                context.Todos.Update(todo);
                await context.SaveChangesAsync(cancellationToken);
            }

        }

        public async Task DeleteAsync(Guid? accountId, Todo todo, CancellationToken cancellationToken = default)
        {
            if (todo.AccountId == accountId)
            {
                context.Todos.Remove(todo);
                await context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task DeleteBulkAsync(Guid? accountId, List<Todo> todos, CancellationToken cancellationToken = default)
        {
            if (todos.Any(todo => todo.AccountId != accountId))
            {
                throw new UnauthorizedAccessException("Some Todos do not belong to the specified account.");
            }
            context.Todos.RemoveRange(todos);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
