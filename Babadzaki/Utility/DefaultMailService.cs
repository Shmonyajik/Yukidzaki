using System.Net;
using System.Net.Mail;//устаревший 

namespace Babadzaki.Utility
{
       
    public class DefaultMailService: IMailService
    { 
        private readonly ILogger<DefaultMailService> _logger;
        public DefaultMailService(ILogger<DefaultMailService> logger)
        {
            _logger = logger;
        }

        public void SendMessage()
        {
            try
            {
                MailMessage message = new MailMessage();
                message.IsBodyHtml = true;
                message.From = new MailAddress("vjxfkrf2000@gmail.com", "Google");
                message.To.Add(new MailAddress("naugolniidi@zdohrana.ru"));
                message.Subject = "Message from System.Net.Mail";
                message.Body = "<div style=\"color:red;\">Привет от Babadzaki</div>";
                //message.Attachments.Add(new Attachment("path"));

                using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com"))
                {
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential("vjxfkrf2000@gmail.com", "rqdxhffvhumrldqt");
                    smtpClient.Port = 587;
                    smtpClient.EnableSsl = true;
                    smtpClient.Send(message);
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
