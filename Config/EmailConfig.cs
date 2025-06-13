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
        private static readonly ConfigurationBuilder conf = new ConfigurationBuilder();
        private static readonly IConfiguration iconf = conf.AddUserSecrets<EmailConfig>().Build();
        public  string appPassword;
        public  string smtpserver;
        public  string smtpport;
        public  string smtpemail;

        public EmailConfig()
        {
            appPassword = Environment.GetEnvironmentVariable("mail_password");
            smtpserver = iconf.GetSection("mail")["smtpserver"];
            smtpport = iconf.GetSection("mail")["smtpport"];
            smtpemail = iconf.GetSection("mail")["smtpemail"];

        }

    }
}
