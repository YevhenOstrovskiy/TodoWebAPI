using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        public async Task ChangeByIdAsync(Guid id, Todo inputTodo, CancellationToken cancellationToken = default)
        {
            if (inputTodo == null)
            {
                throw new ArgumentNullException(nameof(inputTodo), "Input Todo cannot be null.");
            }

            var todo = await context.Todos.FindAsync(new object[] { id }, cancellationToken);
            
            if (todo == null)
            {
              throw new ArgumentNullException($"Todo with {id} wasn`t found");
            }

            todo.Name = inputTodo.Name;
            todo.IsComplete = inputTodo.IsComplete;

            await context.SaveChangesAsync(cancellationToken);

        }
    }
}
