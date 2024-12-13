using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public interface IAccountService
    {
        Task Register(string userName, string email, string password);
        Task<string> Login(string email, string passwordHash);
        Guid? GetAccountId();
    }
}
