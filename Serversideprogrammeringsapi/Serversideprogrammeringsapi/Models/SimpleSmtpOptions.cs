namespace Serversideprogrammeringsapi.Models
{
    public class SimpleSmtpOptions
    {
        public string Host { get; set; }

        public int Port { get; set; }

        public string DefaultFromEmail { get; set; }

        public string DefaultFromName { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}
