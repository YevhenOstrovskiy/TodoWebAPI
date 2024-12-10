using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    internal class AccountRepository : IAccountRepository
    {
        private static IDictionary<string, Account> accounts = new Dictionary<string, Account>();

        public void Add(Account account)
        {
            accounts[account.Email] = account;
        }

        public Account? GetByEmail(string email)
        {
            return accounts.TryGetValue(email, out var account) ? account : null;  
        }
    }
}
