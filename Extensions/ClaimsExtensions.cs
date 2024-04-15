using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace api.Extensions
{
    public static class ClaimsExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            return user.Claims.SingleOrDefault(c => c.Type.Equals(ClaimTypes.GivenName)).Value;
        }
        public static string GetEmail(this ClaimsPrincipal user)
        {
            return user.Claims.SingleOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;
        }
    }
}