using Microsoft.AspNetCore.Identity;

namespace Serversideprogrammeringsapi.Database.Models
{
    public class ApiUser : IdentityUser<long>, IDatedEntity
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Updated { get; set; }
        public DateTimeOffset? Disabled { get; set; }
        public bool IsEnabled { get; set; } = true;
    }
}
