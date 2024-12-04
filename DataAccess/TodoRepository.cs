using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task CreateAsync(Todo todo, CancellationToken cancellationToken = default)
        {
            await context.Todos.AddAsync(todo, cancellationToken);
            await context.SaveChangesAsync();
        }

    }
}
