using WyvernWatch.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MimeKit;
using MailKit.Net.Smtp;

namespace WyvernWatch.Services.MailService
{

    internal class MailService : IMailService
    {
        private readonly MimeMessage _msg = new();

        public void SendEmail()
        {
            _msg.From.Add(new MailboxAddress("The Admiral", Environment.GetEnvironmentVariable("mail_smtpemail")));

            _msg.To.Add(new MailboxAddress("", Environment.GetEnvironmentVariable("mail_smtpemail")));

            _msg.Subject = "Subject Azure";

            _msg.Body = new TextPart("plain")
            {
                Text = "Azure Function Fired."
            };

            using (var cl = new SmtpClient())
            {
                cl.Connect(Environment.GetEnvironmentVariable("mail_smtpserver"), int.Parse(Environment.GetEnvironmentVariable("mail_smtpport")), false);

                cl.Authenticate(Environment.GetEnvironmentVariable("mail_smtpemail"), Environment.GetEnvironmentVariable("mail_password"));

                cl.Send(_msg);

                cl.Disconnect(true);
            }
        }
    }
}
