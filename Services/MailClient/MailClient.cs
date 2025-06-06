using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WyvernWatch.Config;
using static System.Net.Mime.MediaTypeNames;
using MailKit.Net.Smtp;



namespace WyvernWatch.Services.MailClient
{
    public class MailClient : IMailClient
    {
        private readonly MimeMessage _msg;
        private readonly EmailConfig _emailconf;


        public MailClient()
        {
            _emailconf = new();
            _msg = new();

        }

        public void StartEmail()
        {
            BuildEmail();
            SendEmail();
        }


        public void BuildEmail()
        {
            _msg.From.Add(new MailboxAddress("The Admiral", _emailconf.smtpemail));

            _msg.To.Add(new MailboxAddress("", _emailconf.smtpemail));

            _msg.Subject = "Subject WORKSSSS";

            _msg.Body = new TextPart("plain")
            {
                Text = "YOOOOO IT WORKSSS"
            };
        }
        public void SendEmail()
        {
            using (var cl = new SmtpClient())
            {
                cl.Connect(_emailconf.smtpserver, Int32.Parse(_emailconf.smtpport), false);


                cl.Authenticate(_emailconf.smtpemail, _emailconf.appPassword);

                cl.Send(_msg);
                cl.Disconnect(true);
            }
        }

    }
}