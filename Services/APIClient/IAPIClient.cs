using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WyvernWatch.Services.APIClient
{
    public interface IAPIClient
    {
        Task Fetch();
        Task ProcessData(Stream s);
    }
}
