using DataAccess;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    internal class JwtService(IOptions<AuthSettings> options)
    {
        public string GenerateToken(Account account)
        {
            var claims = new List<Claim>
            {
                new Claim("userName", account.UserName),
                new Claim("email", account.Email),
                new Claim("id", account.Id.ToString())
            };

            var jwtToken = new JwtSecurityToken(
                expires: DateTime.UtcNow.Add(options.Value.Expires),
                claims: claims,
                signingCredentials:
                new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.SecretKey)),
                    SecurityAlgorithms.HmacSha256)
                );
            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}
