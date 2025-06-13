using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WyvernWatch.Interfaces
{
    public interface IAPIClient
    {
        Task Fetch();
        Task GetRepositories(Stream s);
        Task GetRepositoryCommits();
    }
}
