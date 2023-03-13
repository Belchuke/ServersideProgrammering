namespace Serversideprogrammeringsapi.Models
{
    public class EmailSendOptions
    {
        public string[] TargetAddresses { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public string FromEmail { get; set; }

        public string FromName { get; set; }

        public bool BodyIsHtml { get; set; }

        public string? DevData { get; set; }

        public EmailAttachment[] Attachments { get; set; }
    }
}
