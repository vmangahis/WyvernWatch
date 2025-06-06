using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WyvernWatch.Services.MailClient
{
    public interface IMailClient
    {
        void StartEmail();
        void BuildEmail();
        void SendEmail();
    }
}
