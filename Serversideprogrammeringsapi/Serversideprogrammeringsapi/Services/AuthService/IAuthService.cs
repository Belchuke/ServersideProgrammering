using Serversideprogrammeringsapi.Models;
using Serversideprogrammeringsapi.Types;

namespace Serversideprogrammeringsapi.Services.AuthService
{
    public interface IAuthService
    {
        Task<TwoFactoryRequestType> AuthenticateWithCredentialsAsync(CredentialsInput input);

        Task<AuthResultType> AuthenticateTwoFactoryAsync(TwoFactorInput input);

        Task<AuthResultType> AuthenticateWithRefreshTokenAsync(string refreshTokenId);

        Task<RegisterResult> RegisterUserAsync(RegisterUserInput viewModel);

        Task<RegisterResult> GenerateCodeAsync(string username);

        Task<OtpValidationResult> ValidateOTPAsync(OtpValidationInput viewModel);
    }
}
