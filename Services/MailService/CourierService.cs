using WyvernWatch.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit;

namespace WyvernWatch.Services.MailService
{

    internal class CourierService : ICourierService
    {
        private readonly MimeMessage _msg;
        private string? _smtpEmail;
        private int _smtpPort;
        private string? _senderName;
        private string? _subject;
        private string? _smtpServer;
        private string? _appPassword;

        public CourierService()
        {
            _msg = new();
            _smtpEmail = Environment.GetEnvironmentVariable("mail_smtpemail");
            _smtpPort = int.Parse(Environment.GetEnvironmentVariable("mail_smtpport")!);
            _senderName = Environment.GetEnvironmentVariable("mail_sendername");
            _subject = Environment.GetEnvironmentVariable("mail_subject");
            _smtpServer = Environment.GetEnvironmentVariable("mail_smtpserver");
            _appPassword = Environment.GetEnvironmentVariable("mail_password");

        }

        public void SendEmail(string msg)
        {
            string subjectCompose = $"{_subject}{DateTime.Today}";
            _msg.From.Add(new MailboxAddress(_senderName, _smtpEmail));

            _msg.To.Add(new MailboxAddress("", _smtpEmail));

            _msg.Subject = subjectCompose;

            _msg.Body = new TextPart("plain")
            {
                Text = msg
            };

            using (var cl = new SmtpClient())
            {
                cl.Connect(Environment.GetEnvironmentVariable("mail_smtpserver"), _smtpPort, false);

                
                cl.Authenticate(_smtpEmail, _appPassword);

                cl.Send(_msg);

                cl.Disconnect(true);
            }
        }
    }
}
