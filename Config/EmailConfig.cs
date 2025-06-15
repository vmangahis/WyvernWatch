using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WyvernWatch.Config
{
    public  class EmailConfig
    {
        public  string appPassword;
        public  string smtpserver;
        public  string smtpport;
        public  string smtpemail;

        public EmailConfig()
        {
            appPassword = Environment.GetEnvironmentVariable("mail_password")!;
            smtpserver = Environment.GetEnvironmentVariable("mail_smtpserver")!;
            smtpport = Environment.GetEnvironmentVariable("mail_smtpport")!;
            smtpemail = Environment.GetEnvironmentVariable("mail_smtpemail")!;
        }

    }
}
