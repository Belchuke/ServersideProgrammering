using Serversideprogrammeringsapi.Models;
using Newtonsoft.Json;
using Path = System.IO.Path;

namespace Serversideprogrammeringsapi.Services.ExternalContactService
{
    public class DummyMailSenderService : IMailSenderService
    {
        private readonly ILogger<DummyMailSenderService> _logger;

        public DummyMailSenderService(ILogger<DummyMailSenderService> logger)
        {
            _logger = logger;
        }

        public Task SendEmailAsync(EmailSendOptions request)
        {
            var mailDirectory = Path.Combine(Environment.CurrentDirectory, "emails");

            Directory.CreateDirectory(mailDirectory);

            string filename = $"{DateTime.Now:yyyy-MM-dd HH-mm-ss} {string.Join(" ", request.TargetAddresses)}.json";

            _logger.LogInformation("Would send email to {mails}: {subject}", string.Join(", ", request.TargetAddresses), request.Subject);

            string message = $"{request.DevData} \n" + "\n" + JsonConvert.SerializeObject(request);

            File.WriteAllText(Path.Combine(mailDirectory, filename), message);

            return Task.CompletedTask;
        }
    }
}
