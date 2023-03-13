namespace Serversideprogrammeringsapi.Database.Models
{
    public interface IDatedEntity
    {
        public DateTimeOffset Created { get; set; }

        public DateTimeOffset Updated { get; set; }

        public DateTimeOffset? Disabled { get; set; }

        public bool IsEnabled { get; set; }
    }
}
