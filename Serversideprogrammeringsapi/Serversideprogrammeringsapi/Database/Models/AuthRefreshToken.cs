using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Serversideprogrammeringsapi.Database.Models
{
    public class AuthRefreshToken : IDatedEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        public DateTimeOffset Created { get; set; }

        public DateTimeOffset Updated { get; set; }
        public DateTimeOffset? Disabled { get; set; }

        public long UserId { get; set; }
        public ApiUser User { get; set; }

        public bool IsEnabled { get; set; } = true;
    }
}
