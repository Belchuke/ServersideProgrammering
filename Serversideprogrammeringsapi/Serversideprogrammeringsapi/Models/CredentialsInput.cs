namespace Serversideprogrammeringsapi.Models
{
    [GraphQLDescription("Input for signing in")]
    public class CredentialsInput
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }
}
