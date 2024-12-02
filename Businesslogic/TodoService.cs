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
