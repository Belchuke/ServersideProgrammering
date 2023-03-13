namespace Serversideprogrammeringsapi.Types
{
    public class UserType
    {
        public long Id { get; set; }

        public string Username { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string Email { get; set; }

        public List<string>? Roles { get; set; }

        public DateTimeOffset Created { get; set; }

        public DateTimeOffset Updated { get; set; }

        public DateTimeOffset? Disabled { get; set; }

        public bool IsEnabled { get; set; }
    }
}
