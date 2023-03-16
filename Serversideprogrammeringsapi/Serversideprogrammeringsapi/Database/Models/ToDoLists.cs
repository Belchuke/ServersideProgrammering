using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Serversideprogrammeringsapi.Database.Models
{
    public class ToDoLists : IDatedEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        public byte[] DataName { get; set; }
        public byte[] IVName { get; set; }

        public byte[] DataDescription { get; set; }
        public byte[] IVDescription { get; set; }

        public long UserId { get; set; }

        public List<ToDoListIteam>? ListItems { get; set; }

        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Updated { get; set; }
        public DateTimeOffset? Disabled { get; set; }
        public bool IsEnabled { get; set; } = true;
    }
}
