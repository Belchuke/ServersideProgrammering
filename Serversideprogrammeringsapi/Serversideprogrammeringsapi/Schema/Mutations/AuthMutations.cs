using Serversideprogrammeringsapi.Models;
using Serversideprogrammeringsapi.Services.AuthService;
using Serversideprogrammeringsapi.Types;

namespace Serversideprogrammeringsapi.Schema.Mutations
{
    [ExtendObjectType(typeof(Mutation))]
    public class AuthMutations
    {
        public async Task<CredentialResult> SignIn(CredentialsInput input, [Service] IAuthService authService)
        {
            return await authService.AuthenticateWithCredentialsAsync(input);
        }

        [UseProjection]
        public async Task<AuthResultType> ToFactorSignIn(TwoFactorInput input, [Service] IAuthService authService)
        {
            return await authService.AuthenticateTwoFactoryAsync(input);
        }

        [UseProjection]
        public async Task<AuthResultType> Refresh([GraphQLName("refreshTokenId")] string refreshTokenId, [Service] IAuthService authService)
        {
            return await authService.AuthenticateWithRefreshTokenAsync(refreshTokenId);
        }

        public async Task<RegisterResult> RegisterUser(RegisterUserInput input, [Service] IAuthService service)
        {
            return await service.RegisterUserAsync(input);
        }

        public async Task<RegisterResult> GenerateOTP([GraphQLName("username")] string username, [Service] IAuthService service)
        {
            return await service.GenerateCodeAsync(username);
        }

        public async Task<OtpValidationResult> ValidateOTP(OtpValidationInput input, [Service] IAuthService service)
        {
            return await service.ValidateOTPAsync(input);
        }
    }
}
