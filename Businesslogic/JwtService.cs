using DataAccess;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    internal class JwtService
    {
        public string GenerateToken(Account account)
        {
            var jwtToken = new JwtSecurityToken(
                expires: 
                )
        }
    }
}
