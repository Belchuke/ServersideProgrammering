namespace Serversideprogrammeringsapi.Models
{
    public class UpdateToDoListInput
    {
        public long Id { get; set; }

        public string? Name { get; set; }
        public string? Description { get; set; }

        public List<UpdateToDoListItemInput>? Items { get; set; }

        public bool? IsEnabled { get; set; }
        public bool? Delete { get; set; }
    }
}
