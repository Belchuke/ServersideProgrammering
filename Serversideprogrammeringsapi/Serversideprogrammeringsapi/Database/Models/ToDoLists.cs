﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Serversideprogrammeringsapi.Database.Models
{
    public class ToDoLists : IDatedEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        public string Data { get; set; }
        public string Key { get; set; }
        public string IV { get; set; }

        public long UserId { get; set; }
        public ApiUser User { get; set; }

        public List<ToDoListIteam>? ListItems { get; set; }

        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Updated { get; set; }
        public DateTimeOffset? Disabled { get; set; }
        public bool IsEnabled { get; set; } = true;
    }
}
