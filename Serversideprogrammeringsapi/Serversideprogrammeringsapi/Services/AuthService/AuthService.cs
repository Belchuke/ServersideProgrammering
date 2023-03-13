using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serversideprogrammeringsapi.Database.Models;
using Serversideprogrammeringsapi.Identity.JWT;
using Serversideprogrammeringsapi.Identity.Repo;
using Serversideprogrammeringsapi.Models;
using Serversideprogrammeringsapi.Repo.OneTimePasswordRepo;
using Serversideprogrammeringsapi.Services.ExternalContactService;
using Serversideprogrammeringsapi.Types;
using System.Security.Claims;

namespace Serversideprogrammeringsapi.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IUserManager _userManager;
        private readonly IJwtFactory _jwtFactory;
        private readonly IRefreshTokenRepo _refreshTokenRepo;
        private readonly ILogger<AuthService> _logger;
        private readonly IExternalContactService _externalContactService;
        private readonly IOTPRepo _otpRepo;

        public AuthService(IUserManager userManager, IJwtFactory jwtFactory, IRefreshTokenRepo refreshTokenRepo, ILogger<AuthService> logger, IExternalContactService externalContactService, IOTPRepo otpRepo)
        {
            _userManager = userManager;
            _jwtFactory = jwtFactory;
            _refreshTokenRepo = refreshTokenRepo;
            _logger = logger;
            _externalContactService = externalContactService;
            _otpRepo = otpRepo;
        }

        public async Task<CredentialResult> AuthenticateWithCredentialsAsync(CredentialsInput input)
        {
            ApiUser? user = await _userManager.FindByNameAsync(input.Username);

            if (user == null)
            {
                _logger.LogInformation($"Unknown user login attempt - {input.Username}");

                return new CredentialResult()
                {
                    IsSuccessful = false,
                    Message = "Invalid username or password",
                };
            }

            if (!user.IsEnabled)
            {
                _logger.LogInformation($"Attempted login on disabled user {user.UserName} {user.Id}");

                return new CredentialResult()
                {
                    IsSuccessful = false,
                    Message = "User is disabled",
                }; 
            }


            if (!user.EmailConfirmed)
            {
                return new CredentialResult()
                {
                    IsSuccessful = false,
                    Message = "User is not validated",
                };
            }

            string token = await _userManager.GenerateTwoFactorTokenAsync(user);

            await _externalContactService.SendTwoFactorToken(user.UserName, token);

            return new CredentialResult()
            {
                IsSuccessful = true,
                Message = "Token sent"
            };
        }

        public async Task<AuthResultType> AuthenticateTwoFactoryAsync(TwoFactorInput input)
        {
            ApiUser? result = await _userManager.TwoFactorSignInAsync(input);

            if (result == null)
            {
                _logger.LogInformation($"Incorrect two-facotr code passed for user {input.Username}");

                return new AuthResultType()
                {
                    IsSuccessful = false,
                    Message = "Incorrect two-factor code passed"
                };
            }

            IList<string> userRoles = await _userManager.GetRolesAsync(result);

            ClaimsIdentity identity = _jwtFactory.GenerateClaimsIdentity(result.UserName, result.Id, userRoles);

            if (identity == null)
            {
                _logger.LogInformation($"Could not verify the identity of {result.UserName}");

                return new AuthResultType()
                {
                    IsSuccessful = false,
                    Message = "Could not verify the identity"
                };
            }

            AuthRefreshToken? refreshToken = await _refreshTokenRepo.GetByUserIdAsync(result.Id);

            if (refreshToken == null)
            {
                refreshToken = new AuthRefreshToken()
                {
                    UserId = result.Id,
                };

                await _refreshTokenRepo.AddAsync(refreshToken);
            }

            return new AuthResultType()
            {
                Token = await _jwtFactory.GenerateEncodedToken(result.UserName, identity),

                Expires = DateTimeOffset.UtcNow.AddSeconds(_jwtFactory.GetJwtOptions().ValidFor.TotalSeconds),

                RefreshToken = Convert.ToBase64String(refreshToken.Id.ToByteArray()),

                UserId = result.Id,

                User = new UserType()
                {
                    Id = result.Id,

                    Username = result.UserName,

                    Email = result.Email,

                    FirstName = result.FirstName,

                    LastName = result.LastName,

                    Created = result.Created,

                    Updated = result.Updated,

                    Disabled = result.Disabled,

                    IsEnabled = result.IsEnabled,

                    Roles = userRoles.ToList(),
                }
            };
        }

        public async Task<AuthResultType> AuthenticateWithRefreshTokenAsync(string refreshTokenId)
        {
            Guid targetGuid = new(Convert.FromBase64String(refreshTokenId));

            AuthRefreshToken? token = await _refreshTokenRepo.GetByGuidAsync(targetGuid);

            if (token == null)
            {
                _logger.LogWarning($"Unknown refresh token {refreshTokenId} was sent");

                return new AuthResultType()
                {
                    IsSuccessful = false,
                    Message = "Refresh token not found"
                };
            }

            if (!token.IsEnabled)
            {
                _logger.LogWarning($"There was an attempt to use refreshing token: {refreshTokenId} for user {token?.User?.UserName} but it was disabled");

                return new AuthResultType()
                {
                    IsSuccessful = false,
                    Message = "Refresh token is disabled"
                };
            }

            IList<string> userRoles = await _userManager.GetRolesAsync(token.User);


            return new AuthResultType()
            {
                Token = await _jwtFactory.GenerateEncodedToken(token.User.UserName, _jwtFactory.GenerateClaimsIdentity(token.User.UserName, token.User.Id, userRoles)),

                Expires = DateTimeOffset.UtcNow.AddSeconds(_jwtFactory.GetJwtOptions().ValidFor.TotalSeconds),

                RefreshToken = Convert.ToBase64String(token.Id.ToByteArray()),

                UserId = token.User.Id,


                User = new UserType()
                {
                    Id = token.User.Id,

                    Username = token.User.UserName,

                    FirstName = token.User.FirstName,

                    LastName = token.User.LastName,

                    Email = token.User.Email,

                    Created = token.User.Created,

                    Updated = token.User.Updated,

                    Disabled = token.User.Disabled,

                    IsEnabled = token.User.IsEnabled,

                    Roles = userRoles.ToList(),
                }
            };
        }

        public async Task<RegisterResult> RegisterUserAsync(RegisterUserInput input)
        {
            ApiUser? anyUser = await _userManager.FindByEmailAsync(input.UserName);

            if (anyUser != null)
            {
                _logger.LogInformation("Attempt to create account with an email that is already taken");

                return new RegisterResult()
                {
                    IsSuccessful = false,
                    Message = "Email already taken"
                };
            }

            IdentityResult? createdResult = await _userManager.CreateAsync(new ApiUser
            {
                UserName = input.UserName,
                Email = input.UserName,
                EmailConfirmed = false,
                TwoFactorEnabled = true,
            }, input.Password);


            if (!createdResult.Succeeded)
            {
                string reason = string.Join(", ", createdResult.Errors.Select(x => x.Description));

                string desc = $"Failed attempt on sign up with username: {input.UserName}";

                _logger.LogInformation(desc + " " + reason);

                return new RegisterResult()
                {
                    Message = desc + " " + reason,
                    IsSuccessful = false,
                };
            }

            SignupOtp result = await _otpRepo.CreateCodeAsync(input.UserName);

            await _externalContactService.SendOTPAsync(input.UserName, result.Code);

            return new RegisterResult()
            {
                Expires = result.Expiration,
                IsSuccessful = true,
                Message = "Created"
            };
        }


        public async Task<RegisterResult> GenerateCodeAsync(string username)
        {
            SignupOtp result = await _otpRepo.CreateCodeAsync(username);

            await _externalContactService.SendOTPAsync(username, result.Code);

            return new RegisterResult()
            {
                Expires = result.Expiration,
                IsSuccessful = true,
                Message = "Updated"
            };
        }

        public async Task<OtpValidationResult> ValidateOTPAsync(OtpValidationInput input)
        {
            SignupOtp? signupOTPFound = await _otpRepo.GetAsync(input.Username);

            if (signupOTPFound == null)
            {
                return new OtpValidationResult()
                {
                    IsSuccessful = false,
                    Message = "No user found"
                };
            }

            if (_otpRepo.ValidateFailedAttempts(signupOTPFound.FailedAttempts))
            {
                return new OtpValidationResult()
                {
                    IsSuccessful = false,
                    Message = "New code required: to many failed attempts"
                };
            }

            if (_otpRepo.ValidateIfExpired(signupOTPFound.Expiration))
            {
                return new OtpValidationResult()
                {
                    IsSuccessful = false,
                    Message = "New code required: code has expired"
                };
            }

            if (input.Code == signupOTPFound.Code)
            {
                ApiUser getUser = await _userManager.FindByNameAsync(input.Username.ToUpper());

                getUser.EmailConfirmed = true;

                signupOTPFound.Validated = true;

                await _userManager.UpdateAsync(getUser);
                await _otpRepo.UpdateAsync(signupOTPFound);

                return new OtpValidationResult()
                {
                    IsSuccessful = true,
                    Message = "Validated",
                };
            }
            else
            {
                signupOTPFound.FailedAttempts++;
                await _otpRepo.UpdateAsync(signupOTPFound);


                return new OtpValidationResult()
                {
                    IsSuccessful = false,
                    Message = "Incorrect code entered"
                };
            }
        }
    }
}
