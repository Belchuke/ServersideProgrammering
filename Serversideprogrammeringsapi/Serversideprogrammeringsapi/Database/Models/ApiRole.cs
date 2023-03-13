using Microsoft.AspNetCore.Identity;

namespace Serversideprogrammeringsapi.Database.Models
{
    public class ApiRole : IdentityRole<long>
    {
        public ICollection<ApiUserRole> TheUserRolesList { get; set; }
    }
}
