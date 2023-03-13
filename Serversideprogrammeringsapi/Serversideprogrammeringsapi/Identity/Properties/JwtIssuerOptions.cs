using Microsoft.IdentityModel.Tokens;

namespace Serversideprogrammeringsapi.Identity.Properties
{
    public class JwtIssuerOptions
    {
        /// <summary>
        /// "iss" (Issuer) Claim - The "iss" (issuer) claim identifies the principal that issued the JWT.
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// "aud" (Audience) Claim - The "aud" (audience) claim identifies the recipients that the JWT is intended for.
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        ///  "key" (SymmetricSecurityKey) - the "key" specifies the security key in the jwt token.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// "exp" (Expiration Time) Claim - The "exp" (expiration time) claim identifies the expiration time on or after which the JWT MUST NOT be accepted for processing.
        /// </summary>
        public DateTime Expiration => IssuedAt.Add(ValidFor);

        public DateTime ExpirationTrackBox => IssuedAt.Add(ValidForTrackBox);

        /// <summary>
        /// "nbf" (Not Before) Claim - The "nbf" (not before) claim identifies the time before which the JWT MUST NOT be accepted for processing.
        /// </summary>
        public static DateTime NotBefore => DateTime.UtcNow;

        /// <summary>
        /// "iat" (Issued At) Claim - The "iat" (issued at) claim identifies the time at which the JWT was issued.
        /// </summary>
        public static DateTime IssuedAt => DateTime.UtcNow;

        /// <summary>
        /// Set the timespan the token will be valid for (default is 120 min)
        /// </summary>
        public TimeSpan ValidFor { get; set; } = TimeSpan.FromMinutes(120);

        public TimeSpan ValidForTrackBox { get; set; } = TimeSpan.FromHours(24);

        /// <summary>
        /// "jti" (JWT ID) Claim (default ID is a GUID)
        /// </summary>
        public Func<Task<string>> JtiGenerator =>
          () => Task.FromResult(Guid.NewGuid().ToString());

        public SigningCredentials SigningCredentials { get; set; }
    }
}
