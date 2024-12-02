using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Businesslogic
{
    public interface ITodoService
    {
        Task CreateAsync(string name, CancellationToken cancellationToken = default);
    }
}
