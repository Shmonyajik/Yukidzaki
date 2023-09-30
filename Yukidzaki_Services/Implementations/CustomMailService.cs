using System.Net.Mail;
using System.Net;
using MimeKit;

using Bdev.Net.Dns;
using Bdev.Net.Dns.Records;
using Microsoft.Extensions.Logging;
using Yukidzaki_Domain.Responses;
using Yukidzaki_Domain.Models;
using Yukidzaki_Domain.Enums;
using Microsoft.Extensions.Configuration;

namespace Babadzaki_Services
{
    public class CustomMailService : IMailService
    {
        private readonly ILogger<CustomMailService> _logger;
        private readonly IConfiguration _config;
        public CustomMailService(ILogger<CustomMailService> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }
        //TODO: Подумать как нормально реализовать(перегрузка или необязательные параметры)
        public void SendMessage(string to, string from, string subject, string bodyText)
        {
            string DomainName = to.Substring(to.IndexOf('@') + 1);
            MXRecord[] records = Resolver.MXLookup(DomainName);//TODO: обернуть в try cath
            if (records.Count() > 0)
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
                        smtpClient.Connect(_config.GetSection("EmailSettings:SMTPHost").Value, int.Parse(_config.GetSection("EmailSettings:SMTPHostPort").Value), bool.Parse(_config.GetSection("EmailSettings:UseSSL").Value));
                        smtpClient.Authenticate(_config.GetSection("EmailSettings:EmailName").Value, _config.GetSection("EmailSettings:EmailPass").Value);

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

        public void SendMessage(IEnumerable<string> emailList, string from, string subject, string bodyText)
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
                    smtpClient.Connect(_config.GetSection("EmailSettings:SMTPHost").Value, int.Parse(_config.GetSection("EmailSettings:SMTPHostPort").Value), bool.Parse(_config.GetSection("EmailSettings:UseSSL").Value));
                    smtpClient.Authenticate(_config.GetSection("EmailSettings:EmailName").Value, _config.GetSection("EmailSettings:EmailPass").Value);

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

        public async Task<BaseResponse<bool>> SendMessage(Email email, string from, string subject, string bodyText)
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
                    message.Body = new BodyBuilder() { HtmlBody = $"<div style=\"color:red;\">{bodyText ?? ""}</div>" }.ToMessageBody();

                    using (MailKit.Net.Smtp.SmtpClient smtpClient = new MailKit.Net.Smtp.SmtpClient())
                    {
                        await smtpClient.ConnectAsync(_config.GetSection("EmailSettings:SMTPHost").Value, int.Parse(_config.GetSection("EmailSettings:SMTPHostPort").Value), bool.Parse(_config.GetSection("EmailSettings:UseSSL").Value));
                        await smtpClient.AuthenticateAsync(_config.GetSection("EmailSettings:EmailName").Value, _config.GetSection("EmailSettings:EmailPass").Value);

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

        public async Task<BaseResponse<bool>> SendMessage(IEnumerable<Email> emailList, string from, string subject, string bodyText)
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
                    await smtpClient.ConnectAsync(_config.GetSection("EmailSettings:SMTPHost").Value, int.Parse(_config.GetSection("EmailSettings:SMTPHostPort").Value), bool.Parse(_config.GetSection("EmailSettings:UseSSL").Value)); ;
                    await smtpClient.AuthenticateAsync(_config.GetSection("EmailSettings:EmailName").Value, _config.GetSection("EmailSettings:EmailPass").Value);

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
