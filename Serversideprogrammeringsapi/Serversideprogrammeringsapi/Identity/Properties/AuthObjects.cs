namespace Serversideprogrammeringsapi.Identity.Properties
{
    public static class AuthConstants
    {
        public static class JwtClaimIdentifiers
        {
            public const string Rol = "rol", Id = "id";
        }
    }

    public static class AuthPolicies
    {
        public const string RequireAdmin = "policy-admin";
    }

    public static class AuthRoles
    {
        public const string Admin = "admin";

        public static IReadOnlyList<string> List { get; } = new List<string>()
        {
            Admin
        };
    }
}
