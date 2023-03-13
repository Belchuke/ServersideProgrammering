using Serversideprogrammeringsapi.Models;
using Serversideprogrammeringsapi.Types;

namespace Serversideprogrammeringsapi.Services.AuthService
{
    public interface IAuthService
    {
        Task<CredentialResult> AuthenticateWithCredentialsAsync(CredentialsInput input);

        Task<AuthResultType> AuthenticateTwoFactoryAsync(TwoFactorInput input);

        Task<AuthResultType> AuthenticateWithRefreshTokenAsync(string refreshTokenId);

        Task<RegisterResult> RegisterUserAsync(RegisterUserInput input);

        Task<RegisterResult> GenerateCodeAsync(string username);

        Task<OtpValidationResult> ValidateOTPAsync(OtpValidationInput input);
    }
}
