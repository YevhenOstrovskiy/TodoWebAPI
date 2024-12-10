using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public interface IAccountService
    {
        void Register(string userName, string email, string password);
        string Login(string email, string passwordHash);
    }
}
