using Microsoft.EntityFrameworkCore;
using OtpNet;
using Serversideprogrammeringsapi.Database;
using Serversideprogrammeringsapi.Database.Models;
using Serversideprogrammeringsapi.Env;
using Serversideprogrammeringsapi.Identity.Properties;
using System.Text;

namespace Serversideprogrammeringsapi.Repo.OneTimePasswordRepo
{
#pragma warning disable CS8603 // Possible null reference return.

    public class OTPRepo : IOTPRepo
    {
        private readonly IDbContextFactory<ApiDbContext> _dbContextFactory;
        private readonly OTPOptions _otpOptions;

        public OTPRepo(IDbContextFactory<ApiDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
            _otpOptions = new OTPOptions();
        }

        public async Task<SignupOtp>? GetAsync(string username)
        {
            await using ApiDbContext dbContext = _dbContextFactory.CreateDbContext();
            return await dbContext.SignupOtps.FirstOrDefaultAsync(x => x.SentTo.StartsWith(username) && x.IsEnabled);
        }

        public async Task<SignupOtp> GetWithUIdAsync(string username, long userId)
        {
            await using ApiDbContext dbContext = _dbContextFactory.CreateDbContext();
            return await dbContext.SignupOtps.FirstOrDefaultAsync(x => x.SentTo.StartsWith(username) && x.IsEnabled && x.UId == userId);
        }

        public async Task<SignupOtp> CreateCodeAsync(string username)
        {
            await DisableCodeAsync(username);

            string code = GenerateCode(username);
            DateTimeOffset expiration = GenerateExpires();

            SignupOtp signUpOtp = new SignupOtp()
            {
                FailedAttempts = 0,
                Expiration = expiration,
                Code = code,
                SentTo = $"{username}:{code}",
                Validated = false,
                IsEnabled = true,
            };

            await using ApiDbContext dbContext = _dbContextFactory.CreateDbContext();
            await dbContext.SignupOtps.AddAsync(signUpOtp);
            await dbContext.SaveChangesAsync();

            return signUpOtp;
        }

        public async Task UpdateAsync(SignupOtp signupOtp)
        {
            await using ApiDbContext dbContext = _dbContextFactory.CreateDbContext();
            dbContext.SignupOtps.Update(signupOtp);
            await dbContext.SaveChangesAsync();
        }

        public async Task DisableCodeAsync(string username)
        {
            await using ApiDbContext dbContext = _dbContextFactory.CreateDbContext();
            SignupOtp? get = await dbContext.SignupOtps.FirstOrDefaultAsync(x => x.SentTo.StartsWith(username) && x.IsEnabled);

            if (get != null)
            {
                get.IsEnabled = false;

                await dbContext.SaveChangesAsync();
            }
        }

        public string GenerateCode(string username)
        {
            string secret = EnvHandler.GETOTPKey();
            byte[] combinedSecret = Encoding.ASCII.GetBytes(string.Concat(secret, username));

            Hotp hotp = new Hotp(combinedSecret, mode: OtpHashMode.Sha512);
            return hotp.ComputeHOTP(DateTime.Now.Ticks);
        }

        public DateTimeOffset GenerateExpires()
        {
            return DateTime.UtcNow.AddMinutes(_otpOptions.OTPValidFor);
        }

        public int GetAttemptsLimit()
        {
            return _otpOptions.AttemptsLimit;
        }

        public bool ValidateIfExpired(DateTimeOffset expiration)
        {
            return expiration < DateTimeOffset.UtcNow;
        }

        public bool ValidateFailedAttempts(int failedAttempts)
        {
            return failedAttempts >= GetAttemptsLimit();
        }
    }

#pragma warning restore CS8603 // Possible null reference return.
}
