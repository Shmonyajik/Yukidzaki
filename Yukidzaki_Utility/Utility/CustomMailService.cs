using System.Net.Mail;
using System.Net;
using MimeKit;

using Bdev.Net.Dns;
using Bdev.Net.Dns.Records;
using Microsoft.Extensions.Logging;

namespace Babadzaki_Utility
{
    public class CustomMailService : IMailService
    {
        private readonly ILogger<CustomMailService> _logger;

        public CustomMailService(ILogger<CustomMailService> logger)
        {
            _logger = logger;
        }
        //TODO: Подумать как нормально реализовать(перегрузка или необязательные параметры)
        public void SendMessage(string to, string from = "vjxfkrf2000@gmail.com", string subject = "", string bodyText = "")
        {
            string DomainName = to.Substring(to.IndexOf('@')+1);
            MXRecord[] records = Resolver.MXLookup(DomainName);//TODO: обернуть в try cath
            if (records.Count()>0)
            {
                try
                {
                    MimeMessage message = new MimeMessage();

                    message.From.Add(new MailboxAddress(WebConstants.CompanyName, from));
                    message.To.Add(new MailboxAddress($"Client_{Guid.NewGuid()}", to));
                    message.Subject = subject;
                    message.Body = new BodyBuilder() { HtmlBody = $"<div style=\"color:red;\">{bodyText}</div>" }.ToMessageBody();
                    //message.Attachments.Add(new Attachment("path"));

                    using (MailKit.Net.Smtp.SmtpClient smtpClient = new MailKit.Net.Smtp.SmtpClient())
                    {
                        smtpClient.Connect(WebConstants.SmtpHost, WebConstants.SmtpHostPort, WebConstants.UseSsl);
                        smtpClient.Authenticate(WebConstants.EmailFrom, WebConstants.EmailPass);

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
            else
            {
                _logger.LogError($"{DomainName} не содержит MX записей!");
            }
            
        }

        public void SendMessage(IEnumerable<string> emailList, string from = "vjxfkrf2000@gmail.com", string subject = "", string bodyText = "")
        {
            
                try
                {
                    MimeMessage message = new MimeMessage();

                    message.From.Add(new MailboxAddress(WebConstants.CompanyName, from));

                    foreach (string email in emailList)
                    {
                        message.To.Add(new MailboxAddress($"Client_{Guid.NewGuid().ToString()}", email));
                    }
                    message.Subject = subject;
                    message.Body = new BodyBuilder() { HtmlBody = $"<div style=\"color:red;\">{bodyText}</div>" }.ToMessageBody();
                    //message.Attachments.Add(new Attachment("path"));

                    using (MailKit.Net.Smtp.SmtpClient smtpClient = new MailKit.Net.Smtp.SmtpClient())
                    {
                        smtpClient.Connect(WebConstants.SmtpHost, WebConstants.SmtpHostPort, WebConstants.UseSsl);
                        smtpClient.Authenticate(WebConstants.EmailFrom, WebConstants.EmailPass);

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
