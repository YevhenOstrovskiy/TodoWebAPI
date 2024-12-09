using DataAccess;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    internal class AccountService(IAccountRepository accountRepository) : IAccountService
    {
        public void Register(string userName, string email, string password)
        {
            var account = new Account
            {
                Id = Guid.NewGuid(),
                UserName = userName,
                Email = email,
            };
            var passwordHash = new PasswordHasher<Account>().HashPassword(account, password);
            account.PasswordHash = passwordHash;

            accountRepository.Add(account);
        }
        public void Login(string email, string password)
        {
            var account = accountRepository.GetByUsername(email);
            var result = new PasswordHasher<Account>()
                .VerifyHashedPassword(
                    account, account.PasswordHash, password
                );
            if (result == PasswordVerificationResult.Success)
            {
                //generate token
            }
            else 
            {
                throw new Exception("Unauthorized");
            }

        }

    }
}
