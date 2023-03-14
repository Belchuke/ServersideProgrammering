namespace Serversideprogrammeringsapi.Models
{
    public class CreateToDoListItemInput
    {
        [GraphQLDescription("Only set value updating or creating standalone item ")]
        public long? ToDoListId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
    }
}
