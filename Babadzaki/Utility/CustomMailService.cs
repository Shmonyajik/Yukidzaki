using System.Net.Mail;
using System.Net;
using MimeKit;

namespace Babadzaki.Utility
{
    public class CustomMailService : IMailService
    {
        private readonly ILogger<CustomMailService> _logger;
        public CustomMailService(ILogger<CustomMailService> logger)
        {
            _logger = logger;
        }

        public void SendMessage()
        {
            try
            {
                MimeMessage message = new MimeMessage();
                
                message.From.Add(new MailboxAddress("Google", "vjxfkrf2000@gmail.com"));
                message.To.Add(new MailboxAddress("Zimbra","naugolniidi@zdohrana.ru"));
                message.Subject = "Message from MailKit";
                message.Body =new BodyBuilder() { HtmlBody = "<div style=\"color:red;\">Привет от Babadzaki</div>" }.ToMessageBody();
                //message.Attachments.Add(new Attachment("path"));

                using (MailKit.Net.Smtp.SmtpClient smtpClient = new MailKit.Net.Smtp.SmtpClient())
                {
                    smtpClient.Connect("smtp.gmail.com", 587, true);
                    smtpClient.Authenticate("vjxfkrf2000@gmail.com", "rqdxhffvhumrldqt");
                    
                    smtpClient.Send(message);
                    smtpClient.Disconnect(true);

                }
                _logger.LogInformation("Message sent successfully!");
            }
            catch (Exception ex)
            {

                _logger.LogError($"Sending message failed! {ex}");
            }
        }
    }
}
