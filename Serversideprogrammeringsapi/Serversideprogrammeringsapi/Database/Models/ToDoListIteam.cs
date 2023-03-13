using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Serversideprogrammeringsapi.Database.Models
{
    public class ToDoListIteam : IDatedEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        public string DataName { get; set; }
        public string KeyName { get; set; }
        public string IVName { get; set; }

        public string DataDescription { get; set; }
        public string KeyDescription { get; set; }
        public string IVDescription { get; set; }

        public long ToDoListId { get; set; }
        public ToDoLists ToDoList { get; set; }

        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Updated { get; set; }
        public DateTimeOffset? Disabled { get; set; }
        public bool IsEnabled { get; set; } = true;
    }
}
