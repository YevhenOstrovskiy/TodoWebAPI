using DataAccess;
using System;
using System.Collections.Generic;

namespace Businesslogic
{
    public interface ITodoService
    {
        Task<List<Todo>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<List<Todo>> GetCompletedAsync(CancellationToken cancellationToken = default);
        Task<List<Todo>> GetUncompletedAsync(CancellationToken cancellationToken = default);
        Task<Todo?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<Todo>?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
        Task CreateAsync(string name, CancellationToken cancellationToken = default);
        Task CreateBulkAsync(List<string> newTodoNames, CancellationToken cancellationToken = default);
        Task UpdateByIdAsync(Guid id, Todo inputTodo, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task DeleteBulkAsync(List<Todo> todos, CancellationToken cancellationToken = default);
        
    }

}
