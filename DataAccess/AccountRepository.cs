using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    internal class AccountRepository(TodoDb context) : IAccountRepository
    {

        public async Task AddAsync(Account account, CancellationToken cancellationToken = default)
        {
            await context.Accounts.AddAsync(account, cancellationToken);
            await context.SaveChangesAsync();
        }

        public async Task<Account> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await context.Accounts.FirstOrDefaultAsync(a => a.Email == email);
        }
    }
}
