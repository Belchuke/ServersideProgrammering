namespace Serversideprogrammeringsapi.Models
{
    public class UpdateToDoListItemInput
    {
        [GraphQLDescription("Set value when updating, leave when creating")]
        public long? Id { get; set; }

        [GraphQLDescription("set value when creating, leave when update")]
        public long? ListId { get; set; }

        public bool AddNewItem { get; set; }

        public string? Name { get; set; }
        public string? Description { get; set; }

        public bool? IsEnabled { get; set; }
        public bool? Delete { get; set; }
    }
}
