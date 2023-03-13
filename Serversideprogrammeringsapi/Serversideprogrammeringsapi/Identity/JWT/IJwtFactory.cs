using Serversideprogrammeringsapi.Identity.Properties;
using System.Security.Claims;

namespace Serversideprogrammeringsapi.Identity.JWT
{
    public interface IJwtFactory
    {
        Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity);

        ClaimsIdentity GenerateClaimsIdentity(string userName, long id, IEnumerable<string> roles);

        JwtIssuerOptions GetJwtOptions();
    }
}
