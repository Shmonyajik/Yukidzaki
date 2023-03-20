using Babadzaki.Models;
using MimeKit;
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


        public void SendMessage(string to, string from = "vjxfkrf2000@gmail.com", string subject="", string bodyText="")
        {
            try
            {
                MailMessage message = new MailMessage();
                message.IsBodyHtml = true;
                message.From = new MailAddress(from, WebConstants.CompanyName);
                message.To.Add(new MailAddress(to));
                message.Subject = subject;
                message.Body = $"<div style=\"color:red;\">{bodyText}</div>";
                //message.Attachments.Add(new Attachment("path"));

                using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com"))
                {
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(WebConstants.EmailFrom, WebConstants.EmailPass);
                    smtpClient.Port = WebConstants.SmtpHostPort;
                    smtpClient.EnableSsl = WebConstants.UseSsl;
                    smtpClient.Send(message);
                }
                _logger.LogInformation("Message sent successfully!");
            }
            catch (Exception ex)
            {

                _logger.LogError($"Sending message failed! {ex}");
            }
        }

        public void SendMessage(IEnumerable<Email> emailList, string from = "vjxfkrf2000@gmail.com", string subject = "", string bodyText = "")
        {
            try
            {
                MailMessage message = new MailMessage();
                message.IsBodyHtml = true;
                message.From = new MailAddress(from, WebConstants.CompanyName);
                
                foreach (Email email in emailList)
                {
                    message.To.Add(new MailAddress(email.Name));
                    
                }
                message.Subject = subject;
                message.Body = $"<div style=\"color:red;\">{bodyText}</div>";
                //message.Attachments.Add(new Attachment("path"));

                using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com"))
                {
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(WebConstants.EmailFrom, WebConstants.EmailPass);
                    smtpClient.Port = WebConstants.SmtpHostPort;
                    smtpClient.EnableSsl = WebConstants.UseSsl;
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
