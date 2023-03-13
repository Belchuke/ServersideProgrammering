using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serversideprogrammeringsapi.Database.Models;
using Serversideprogrammeringsapi.Models;

namespace Serversideprogrammeringsapi.Identity.Repo
{
#pragma warning disable CS8603 // Possible null reference return.
    public class UserManager : IUserManager
    {
        private readonly UserManager<ApiUser> _userManager;
        private readonly SignInManager<ApiUser> _signInManager;

        public UserManager(UserManager<ApiUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApiUser> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<ApiUser> FindByNameAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        public async Task<ApiUser> GetUserByIdAsync(long id)
        {
            return await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IList<string>> GetRolesAsync(ApiUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<string> GenerateTwoFactorTokenAsync(ApiUser user)
        {
            return await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
        }

        public async Task<ApiUser?>? TwoFactorSignInAsync(TwoFactorInput input)
        {
            SignInResult result = await _signInManager.TwoFactorSignInAsync("Email", input.Code, false, false);

            if (result.Succeeded)
            {
                return await _userManager.FindByNameAsync(input.Username);
            }
            else
            {
                return null;
            }
        }

        public async Task<IdentityResult> CreateAsync(ApiUser apiUser, string password)
        {
            return await _userManager.CreateAsync(apiUser, password);
        }

        public async Task<IdentityResult> UpdateAsync(ApiUser apiUser)
        {
            return await _userManager.UpdateAsync(apiUser);
        }

        public async Task<IdentityResult> ChangePasswordAsync(ApiUser user, string currentPassword, string newPassword)
        {
            return await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        }

        public async Task<bool> CheckPasswordAsync(ApiUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(ApiUser apiUser)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(apiUser);
        }

        public async Task<IdentityResult> ResetPasswordAsync(ApiUser apiUser, string token, string newPassword)
        {
            return await _userManager.ResetPasswordAsync(apiUser, token, newPassword);
        }

        public async Task<IdentityResult> SetPasswordAsync(ApiUser apiUser, string password)
        {
            string token = await _userManager.GeneratePasswordResetTokenAsync(apiUser);

            return await _userManager.ResetPasswordAsync(apiUser, token, password);
        }

    }

#pragma warning restore CS8603 // Possible null reference return.
}
