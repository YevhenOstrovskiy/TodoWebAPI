using DataAccess;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    internal class AccountService(IAccountRepository accountRepository, JwtService jwtService) : IAccountService
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

            accountRepository.AddAsync(account);
        }
        public string Login(string email, string password)
        {
            var account = accountRepository.GetByEmailAsync(email);

            if (account is null)
            {
                throw new Exception("Account wasn`t found");
            }
            else 
            {
                var result = new PasswordHasher<Account>()
                .VerifyHashedPassword(
                    account, account.PasswordHash, password
                );
                if (result == PasswordVerificationResult.Success)
                {
                    return jwtService.GenerateToken(account);
                }
                else
                {
                    throw new Exception("Unauthorized");
                }
            }

        }

    }
}
