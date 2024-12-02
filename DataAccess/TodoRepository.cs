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
        public async Task CreateAsync(Todo todo, CancellationToken cancellationToken = default)
        {
            await context.Todos.AddAsync(todo, cancellationToken);
            await context.SaveChangesAsync();
        }
    }
}
