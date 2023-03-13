using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Serversideprogrammeringsapi.Database.Models;
using Serversideprogrammeringsapi.Identity.JWT;
using Serversideprogrammeringsapi.Identity.Properties;

namespace Serversideprogrammeringsapi.Identity
{

    // https://www.yogihosting.com/aspnet-core-identity-two-factor-authentication/
    public class IdentityConfiguration
    {
        private const string SecretKey = "SWWhpFEwnjyRGLmKE4DHSqv7VxlpLY5JneQZHvhmCOK9jgRohBU7Ac4FoCY3d8RGyZ+j88Es+jhiCWdEX5oy1g==";
        private static readonly SymmetricSecurityKey _signingKey = new(Convert.FromBase64String(SecretKey));

        public static void Configure(IConfiguration configuration, IServiceCollection services)
        {
            services.AddSingleton<IJwtFactory, JwtFactory>();

            IConfigurationSection jwtAppSettingOptions = configuration.GetSection(nameof(JwtIssuerOptions));

            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            });

            TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,

                NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, configureOptions =>
            {
                configureOptions.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                configureOptions.TokenValidationParameters = tokenValidationParameters;
                configureOptions.SaveToken = true;
            });

            // define authorization policies specify what roles are allowed to access each policy
            services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthPolicies.RequireAdmin, policy => policy.RequireClaim(AuthConstants.JwtClaimIdentifiers.Rol, AuthRoles.Admin));
            });

            IdentityBuilder builder = services.AddIdentityCore<ApiUser>(o =>
            {
                // configure identity options
                o.Password.RequireDigit = UserManagerOptions.RequireDigit;
                o.Password.RequireLowercase = UserManagerOptions.RequireLowercase;
                o.Password.RequireUppercase = UserManagerOptions.RequireUppercase;
                o.Password.RequireNonAlphanumeric = UserManagerOptions.RequireNonAlphanumeric;
                o.Password.RequiredLength = UserManagerOptions.RequiredLength;
                o.User.AllowedUserNameCharacters = UserManagerOptions.AllowedUserNameCharacters;
                o.SignIn.RequireConfirmedEmail = UserManagerOptions.RequireConfirmedEmail;
                o.SignIn.RequireConfirmedAccount = UserManagerOptions.RequireConfirmedAccount;
            });

            builder = new IdentityBuilder(builder.UserType, typeof(ApiRole), builder.Services);
            builder.AddRoles<ApiRole>();
            builder.AddRoleManager<RoleManager<ApiRole>>();
            builder.AddRoleValidator<RoleValidator<ApiRole>>();
            builder.AddEntityFrameworkStores<ApiDbContext>();
            builder.AddDefaultTokenProviders();

            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithExposedHeaders("X-Pagination")));
        }
    }
}
