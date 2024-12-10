using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface IAccountRepository
    {
        Task AddAsync(Account account, CancellationToken cancellationToken = default);
        Account GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    }
}
