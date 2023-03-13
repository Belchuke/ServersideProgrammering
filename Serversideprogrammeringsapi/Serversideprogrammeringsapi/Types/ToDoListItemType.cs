namespace Serversideprogrammeringsapi.Types
{
    public class ToDoListItemType
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public long ToDoListId { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Updated { get; set; }
        public DateTimeOffset? Disabled { get; set; }
        public bool IsEnabled { get; set; } = true;
    }
}
