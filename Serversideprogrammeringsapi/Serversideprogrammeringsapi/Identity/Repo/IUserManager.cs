using Microsoft.AspNetCore.Identity;
using Serversideprogrammeringsapi.Database.Models;
using Serversideprogrammeringsapi.Models;

namespace Serversideprogrammeringsapi.Identity.Repo
{
    public interface IUserManager
    {
        Task<ApiUser> FindByEmailAsync(string email);
        Task<ApiUser> FindByNameAsync(string userName);
        Task<ApiUser> GetUserByIdAsync(long id);

        Task<IList<string>> GetRolesAsync(ApiUser user);

        Task<bool> CheckPasswordAsync(ApiUser user, string password);

        Task<string> GenerateTwoFactorTokenAsync(ApiUser user);
        Task<ApiUser?>? TwoFactorSignInAsync(TwoFactorInput input);


        Task<IdentityResult> CreateAsync(ApiUser apiUser, string password);
        Task<IdentityResult> UpdateAsync(ApiUser apiUser);


        Task<IdentityResult> SetPasswordAsync(ApiUser apiUser, string password);
        Task<string> GeneratePasswordResetTokenAsync(ApiUser apiUser);

        Task<IdentityResult> ChangePasswordAsync(ApiUser user, string currentPassword, string newPassword);
        Task<IdentityResult> ResetPasswordAsync(ApiUser apiUser, string token, string newPassword);
    }
}
