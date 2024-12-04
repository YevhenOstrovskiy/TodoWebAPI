using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;

namespace Businesslogic
{
    internal class TodoService(ITodoRepository todoRepository) : ITodoService
    {
        public async Task<List<Todo>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await todoRepository.GetAllAsync(cancellationToken);
        }

        public async Task<List<Todo>> GetCompletedAsync(CancellationToken cancellationToken = default)
        {
            return await todoRepository.GetCompletedAsync(cancellationToken);
        }

        public async Task<List<Todo>> GetUncompletedAsync(CancellationToken cancellationToken = default)
        {
            return await todoRepository.GetUncompletedAsync(cancellationToken);
        }

        public async Task<Todo> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var todo = await todoRepository.GetByIdAsync(id, cancellationToken);
            return todo;
        }

        public async Task CreateAsync(string name, CancellationToken cancellationToken = default)
        {
            var todo = new Todo
            {
                Name = name,
            };

            await todoRepository.CreateAsync(todo, cancellationToken);
        }
    }
}
