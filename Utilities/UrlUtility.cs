using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WyvernWatch.Utilities
{
    public static class UrlUtility
    {
        public  static string GetProjectName(string url)
        {
            Uri u = new Uri(url);
            string[] s = u.Segments;
            return s[3].Trim('/');
        }
    }
}
