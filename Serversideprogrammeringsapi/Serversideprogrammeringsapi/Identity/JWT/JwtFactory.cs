using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serversideprogrammeringsapi.Identity.Properties;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace Serversideprogrammeringsapi.Identity.JWT
{
    public class JwtFactory : IJwtFactory
    {
        private readonly JwtIssuerOptions _jwtOptions;

        public JwtFactory(IOptions<JwtIssuerOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }

        public static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() -
                       new DateTime(1970, 1, 1, 0, 0, 0))
                      .TotalSeconds);

        public async Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity)
        {
            SymmetricSecurityKey securtityKey = new(Convert.FromBase64String(_jwtOptions.Key));

            SigningCredentials credentials = new SigningCredentials(securtityKey, SecurityAlgorithms.HmacSha256);

            List<Claim> getClaims = await ReturnClaims(userName, identity);

            JwtSecurityToken jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: getClaims,
                notBefore: JwtIssuerOptions.NotBefore,
                expires: _jwtOptions.Expiration,
                signingCredentials: credentials);

            string encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        public async Task<List<Claim>> ReturnClaims(string username, ClaimsIdentity identity)
        {
            List<Claim> claims = new List<Claim>()
            {
                 new Claim(JwtRegisteredClaimNames.Sub, username),
                 new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
                 new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(JwtIssuerOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                 identity.FindFirst(AuthConstants.JwtClaimIdentifiers.Id),
            };

            claims.AddRange(identity.FindAll(x => x.Type == AuthConstants.JwtClaimIdentifiers.Rol));

            return claims;
        }

        public ClaimsIdentity GenerateClaimsIdentity(string userName, long id, IEnumerable<string> roles)
        {
            List<Claim> claims = new List<Claim>() {
                new Claim(AuthConstants.JwtClaimIdentifiers.Id, id.ToString()),
            };

            foreach (string role in roles)
            {
                claims.Add(new Claim(AuthConstants.JwtClaimIdentifiers.Rol, role));
            }

            return new ClaimsIdentity(new GenericIdentity(userName, "Token"), claims);
        }

        public JwtIssuerOptions GetJwtOptions()
        {
            return _jwtOptions;
        }
    }
}
