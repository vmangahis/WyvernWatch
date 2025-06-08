using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WyvernWatch.Models;

namespace WyvernWatch.Services.APIClient
{
    public class APIClient : IAPIClient
    {
        private readonly ConfigurationBuilder _conf;
        private readonly IConfiguration _iconf;
        private readonly HttpClient? _httpClient;
        private string _apiKey = "";
        private string dateNow;
        private string dateTom;
        private string apiUrl;
        private string apiUrlDate;
        private string appName;

        public APIClient()
        {
            _conf = new();
            _iconf = _conf.AddUserSecrets<APIClient>().Build();
            _apiKey = _iconf.GetSection("github")["publicTk"];
            _httpClient = new();
            dateNow = DateTime.Now.Date.ToString("s", CultureInfo.InvariantCulture);
            dateTom = DateTime.Now.Date.AddDays(1).ToString("s", CultureInfo.InvariantCulture);
            apiUrl = _iconf.GetRequiredSection("github")["url"];
            appName = _iconf.GetRequiredSection("app")["appName"];
        }

        public async Task Fetch()
        {
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.github+json");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            BuildDateQuery();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", appName);
                await using Stream st = await _httpClient.GetStreamAsync(apiUrlDate);

           await ProcessData(st);
        }

        public async Task ProcessData(Stream s)
        {
            var repo = await JsonSerializer.DeserializeAsync<List<Repositories>>(s);

            foreach(var r in repo ?? Enumerable.Empty<Repositories>())
            {
                Console.WriteLine(r);
            }
        }

        private void BuildDateQuery()
        {
            apiUrlDate = String.Format("{0}&since={1}&before={2}",apiUrl,dateNow,dateTom);
            
        }
    }
}