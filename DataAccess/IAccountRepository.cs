using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface IAccountRepository
    {
        void Add(Account account);
        Account GetByEmail(string email);
    }
}
