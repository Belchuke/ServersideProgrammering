namespace Serversideprogrammeringsapi.Services.ExternalContactService
{
    public interface IExternalContactService
    {
        Task SendTwoFactorToken(string username, string token);

        Task SendOTPAsync(string username, string code);
    }
}
