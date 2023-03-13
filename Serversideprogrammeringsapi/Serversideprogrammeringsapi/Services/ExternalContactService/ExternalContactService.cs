namespace Serversideprogrammeringsapi.Services.ExternalContactService
{
    public class ExternalContactService : IExternalContactService
    {
        private readonly IMailSenderService _emailsender;

        public ExternalContactService(IMailSenderService emailsender)
        {
            _emailsender = emailsender;
        }

        public async Task SendOTPAsync(string username, string code)
        {
            await _emailsender.SendEmailAsync(new Models.EmailSendOptions()
            {
                TargetAddresses = new[] { username },
                BodyIsHtml = true,
                DevData = code,
                Subject = "One time password",
                Body = $@"Hello {username}<br>
Here is your singup code<br>
<br>
{code}<br>
<br>
Have a great day<br>
Best regards,<br>
Jacob & Janus",
            });
        }

        public async Task SendTwoFactorToken(string username, string token)
        {
            await _emailsender.SendEmailAsync(new Models.EmailSendOptions()
            {
                TargetAddresses = new[] { username },
                BodyIsHtml = true,
                DevData = token,
                Subject = "Two factory authentication code",
                Body = $@"Hello {username}<br>
Here is your Two-Factor authentication code<br>
<br>
{token}<br>
<br>
Have a great day<br>
Best regards,<br>
Jacob & Janus",
            });
        }
    }
}
