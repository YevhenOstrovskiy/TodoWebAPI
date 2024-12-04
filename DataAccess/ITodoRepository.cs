using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface ITodoRepository
    {
        Task<List<Todo>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<List<Todo>> GetCompletedAsync(CancellationToken cancellationToken = default);
        Task<List<Todo>> GetUncompletedAsync(CancellationToken cancellationToken = default);
        Task<Todo?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        
        //TODO: FindByNameAsync
        
        Task CreateAsync(Todo todo, CancellationToken cancellationToken = default);

    }
}
