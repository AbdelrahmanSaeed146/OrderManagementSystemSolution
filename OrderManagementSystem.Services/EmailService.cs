using Microsoft.Extensions.Options;
using MimeKit;
using OrderManagementSystem.Core;
using OrderManagementSystem.Core.Services;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using MailKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Services
{
    public class EmailService /*: IEmailService*/
    {
    //    private readonly IOptions<EmailStamp> _options;

    //    public EmailService(IOptions<EmailStamp> options)
    //    {
    //        _options = options;
    //    }
    //    public Task SendEmailAsync(string email, string subject, string body)
    //    {
    //        var mail = new MimeMessage()
    //        {
    //            Sender = MailboxAddress.Parse(_options.Value.FromEmail),
    //            Subject = subject,

    //        };
    //        mail.From.Add(new MailboxAddress(_options.Value.Username, _options.Value.FromEmail));



    //        mail.To.Add(MailboxAddress.Parse(toEmail));

    //        var builder = new BodyBuilder();

    //        builder.TextBody = body;


    //        mail.Body = builder.ToMessageBody();

    //        var smtp = new SmtpClient();

    //        smtp.Connect(_options.Value.Host, _options.Value.Port, _options.Value.EnableSsl);

    //        smtp.Authenticate(_options.Value.FromEmail, _options.Value.Password);

    //        smtp.Send(mail);

    //        smtp.Disconnect(true);
    //        mail.To.Clear();
    //    }
    }
}
