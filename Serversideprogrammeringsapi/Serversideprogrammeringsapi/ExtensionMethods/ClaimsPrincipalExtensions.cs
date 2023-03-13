using Serversideprogrammeringsapi.Identity.Properties;
using System.Security.Claims;

namespace Serversideprogrammeringsapi.ExtensionMethods
{
    public static class ClaimsPrincipalExtensions
    {
        public static bool HasRole(this ClaimsPrincipal claims, string role)
        {
            return claims.HasClaim(AuthConstants.JwtClaimIdentifiers.Rol, role);
        }

        public static bool IsAdmin(this ClaimsPrincipal claims)
        {
            return claims.HasRole(AuthRoles.Admin);
        }

        public static long GetUserId(this ClaimsPrincipal claims)
        {
            return Convert.ToInt64(claims.FindFirstValue(AuthConstants.JwtClaimIdentifiers.Id));
        }
    }
}
