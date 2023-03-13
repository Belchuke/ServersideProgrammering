namespace Serversideprogrammeringsapi.Models
{
    public class EmailAttachment
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public byte[] Content { get; set; }
    }
}
