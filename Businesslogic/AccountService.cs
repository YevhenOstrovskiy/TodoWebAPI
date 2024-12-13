using DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    internal class AccountService(IAccountRepository accountRepository, JwtService jwtService, IHttpContextAccessor httpContextAccessor) : IAccountService
    {

        public async Task Register(string userName, string email, string password)
        {
            var account = new Account
            {
                Id = Guid.NewGuid(),
                UserName = userName,
                Email = email,
            };
            var passwordHash = new PasswordHasher<Account>().HashPassword(account, password);
            account.PasswordHash = passwordHash;

            await accountRepository.AddAsync(account);
        }
        public async Task<string> Login(string email, string password)
        {
            var account = await accountRepository.GetByEmailAsync(email);

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

        public Guid? GetAccountId()
        {
            var user = httpContextAccessor.HttpContext?.User;
            if (user is null || !user.Identity.IsAuthenticated)
            {
                return null;
            }

            var accountIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
            return accountIdClaim != null ? Guid.Parse(accountIdClaim.Value) : null;
        }
    }
}
