namespace Serversideprogrammeringsapi.Identity.Properties
{
    public class UserManagerOptions
    {
        //https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity-configuration?view=aspnetcore-7.0#:~:text=IdentityOptions.Password%20specifies%20the%20PasswordOptions%20with%20the%20properties%20shown%20in%20the%20table.

        /// <summary>
        /// If password requires to contains a Digit
        /// </summary>
        public const bool RequireDigit = false;

        /// <summary>
        /// If password requires atleast one lowercase letter
        /// </summary>
        public const bool RequireLowercase = false;

        /// <summary>
        /// If password requires atleast one uppercase letter
        /// </summary>
        public const bool RequireUppercase = false;

        /// <summary>
        /// If Password requires atleast one NonAlphanumeric Character
        /// reffrence: https://study.com/cimages/multimages/16/partial_ascii_table.png
        /// More info: https://theeducationlife.com/alphanumeric-characters/ 
        /// </summary>
        public const bool RequireNonAlphanumeric = false;

        /// <summary>
        /// If password has a minimum requirement for lenght of password
        /// </summary>
        public const int RequiredLength = 6;

        /// <summary>
        /// Charcters which are allowed for the username
        /// </summary>
        public const string AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+()";

        /// <summary>
        /// If its required that to sign in with a email that its confirmed.
        /// </summary>
        public const bool RequireConfirmedEmail = false;

        /// <summary>
        /// If its rquired that to sing in the account is confirmed
        /// </summary>
        public const bool RequireConfirmedAccount = false;
    }
}
