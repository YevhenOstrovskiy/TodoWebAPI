using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface ITodoRepository
    {
        Task CreateAsync(Todo todo, CancellationToken cancellationToken = default);

        Task<Todo?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
