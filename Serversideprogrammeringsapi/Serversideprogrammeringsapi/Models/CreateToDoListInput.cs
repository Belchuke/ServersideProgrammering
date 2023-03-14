namespace Serversideprogrammeringsapi.Models
{
    public class CreateToDoListInput
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<CreateToDoListItemInput>? Items { get; set; }
    }
}
