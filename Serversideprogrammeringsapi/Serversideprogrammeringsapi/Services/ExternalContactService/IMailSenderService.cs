using Serversideprogrammeringsapi.Models;

namespace Serversideprogrammeringsapi.Services.ExternalContactService
{
    public interface IMailSenderService
    {
        Task SendEmailAsync(EmailSendOptions request);
    }
}
