using Azure.Core;
using Bogus.DataSets;
using Microsoft.Extensions.Options;
using Serversideprogrammeringsapi.Env;
using Serversideprogrammeringsapi.Models;
using System.Net.Mail;

namespace Serversideprogrammeringsapi.Services.ExternalContactService
{
    public class SimpleSmtpMailSenderService : IMailSenderService
    {
        private readonly SimpleSmtpOptions _options;

        public SimpleSmtpMailSenderService()
        {
            _options = EnvHandler.GetSmtpOptions();
        }

        public async Task SendEmailAsync(EmailSendOptions request)
        {
            using (MailMessage emailMessage = new MailMessage())
            {
                emailMessage.From = new MailAddress(_options.DefaultFromEmail, _options.DefaultFromName);
                foreach (var address in request.TargetAddresses)
                {
                    emailMessage.To.Add(new MailAddress(address));
                }
                emailMessage.Subject = request.Subject;
                emailMessage.Body = request.Body;
                emailMessage.Priority = MailPriority.Normal;
                emailMessage.IsBodyHtml = true;
                using (SmtpClient MailClient = new SmtpClient(_options.Host, _options.Port))
                {
                    MailClient.EnableSsl = true;
                    MailClient.Credentials = new System.Net.NetworkCredential(_options.Username, _options.Password);
                    await MailClient.SendMailAsync(emailMessage);
                }
            }
        }
    }
}
