using Serversideprogrammeringsapi.Database.Models;

namespace Serversideprogrammeringsapi.Repo.OneTimePasswordRepo
{
    public interface IOTPRepo
    {
        Task<SignupOtp>? GetAsync(string username);

        Task<SignupOtp> GetWithUIdAsync(string username, long userId);

        Task<SignupOtp> CreateCodeAsync(string username);

        Task UpdateAsync(SignupOtp signupOtp);

        Task DisableCodeAsync(string username);

        bool ValidateIfExpired(DateTimeOffset expiration);

        bool ValidateFailedAttempts(int failedAttempts);

        string GenerateCode(string username);

        int GetAttemptsLimit();

        DateTimeOffset GenerateExpires();
    }
}
