using System.Net.Mail;
using System.Net;
using MimeKit;

using Bdev.Net.Dns;
using Bdev.Net.Dns.Records;
using Microsoft.Extensions.Logging;
using Babadzaki_Domain.Responses;
using Babadzaki_Domain.Models;
using Babadzaki_Domain.Enums;

namespace Babadzaki_Services
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

        public async Task<BaseResponse<bool>> SendMessage(Email email, string from, string? subject, string? bodyText)
        {
            var baseResponse = new BaseResponse<bool>();
            try
            {
                string DomainName = email.Name.Substring(email.Name.IndexOf('@') + 1);
                MXRecord[] records = Resolver.MXLookup(DomainName);//TODO: обернуть в try cath
                if (records.Count() > 0)
                {
                    MimeMessage message = new MimeMessage();

                    message.From.Add(new MailboxAddress(WebConstants.CompanyName, from));
                    message.To.Add(new MailboxAddress($"Client_{Guid.NewGuid()}", email.Name));
                    message.Subject = subject ?? "";
                    message.Body = new BodyBuilder() { HtmlBody = $"<div style=\"color:red;\">{bodyText??""}</div>" }.ToMessageBody(); 

                    using (MailKit.Net.Smtp.SmtpClient smtpClient = new MailKit.Net.Smtp.SmtpClient())
                    {
                        await smtpClient.ConnectAsync(WebConstants.SmtpHost, WebConstants.SmtpHostPort, WebConstants.UseSsl);
                        await smtpClient.AuthenticateAsync(WebConstants.EmailFrom, WebConstants.EmailPass);

                        await smtpClient.SendAsync(message);
                        await smtpClient.DisconnectAsync(true);

                    }

                    baseResponse.Data = true;
                    baseResponse.StatusCode = StatusCode.OK;
        
                }
                else
                {
                    baseResponse.Data = false;
                    baseResponse.Description = $"No MX records found for domain {DomainName}";
                    baseResponse.StatusCode = StatusCode.NoMxRecords;
                }
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError,
                    Data = false
                };
            }
            
        }

        public async Task<BaseResponse<bool>> SendMessage(IEnumerable<Email> emailList, string from, string? subject, string? bodyText)
        {
            var baseResponse = new BaseResponse<bool>();
            try
            {
                
                MimeMessage message = new MimeMessage();

                message.From.Add(new MailboxAddress(WebConstants.CompanyName, from));
                message.To.AddRange(emailList.Select(el => new MailboxAddress($"Client_{Guid.NewGuid()}", el.Name)));
                message.Subject = subject ?? "";
                message.Body = new BodyBuilder() { HtmlBody = $"<div style=\"color:red;\">{bodyText ?? ""}</div>" }.ToMessageBody();

                using (MailKit.Net.Smtp.SmtpClient smtpClient = new MailKit.Net.Smtp.SmtpClient())
                {
                    await smtpClient.ConnectAsync(WebConstants.SmtpHost, WebConstants.SmtpHostPort, WebConstants.UseSsl);
                    await smtpClient.AuthenticateAsync(WebConstants.EmailFrom, WebConstants.EmailPass);

                    await smtpClient.SendAsync(message);
                    await smtpClient.DisconnectAsync(true);

                }

                baseResponse.Data = true;
                baseResponse.StatusCode = StatusCode.OK;

                
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError,
                    Data = false
                };
            }
        }
    }


}
