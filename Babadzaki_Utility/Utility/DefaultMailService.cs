
using Bdev.Net.Dns.Records;
using Bdev.Net.Dns;
using MimeKit;
using System.Net;
using System.Net.Mail;//устаревший 
using Microsoft.Extensions.Logging;

namespace Babadzaki_Utility
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
            string DomainName = from.Substring(from.IndexOf('@') + 1);
            MXRecord[] records = Resolver.MXLookup(DomainName);
            if (records.Count() > 0)
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
            else
            {
                _logger.LogError($"{DomainName} не содержит MX записей!");
            }

        }

        public void SendMessage(IEnumerable<string> emailList, string from = "vjxfkrf2000@gmail.com", string subject = "", string bodyText = "")
        {
            
                try
                {
                    MailMessage message = new MailMessage();
                    message.IsBodyHtml = true;
                    message.From = new MailAddress(from, WebConstants.CompanyName);

                    foreach (string email in emailList)
                    {
                        message.To.Add(new MailAddress(email));

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
