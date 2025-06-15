using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WyvernWatch.Interfaces;
using WyvernWatch.Models;

namespace WyvernWatch.Services.APIClient
{
    public class APIClient : IAPIClient
    {
        private readonly ConfigurationBuilder _conf;
        private static HttpClient? _httpClient;
        private string? _apiKey;
        private string dateSince;
        private string dateBefore;
        private string? apiUrl;
        private List<string> ProjectNames;
        private List<RepositoryCommits> CommitUrl;
        private string? apiUrlDate;
        private string? appName;
        private int pageCounter = 1;
        private bool isLastPage = false;
        private string? myGithub;
        private Commit? cm;

        public APIClient()
        {
            _conf = new();
            _apiKey = Environment.GetEnvironmentVariable("github_publicTk");
            _httpClient = new();
            dateBefore = DateTime.Now.Date.AddDays(1).ToString("s", CultureInfo.InvariantCulture);
            dateSince = DateTime.Now.Date.AddDays(-1).ToString("s", CultureInfo.InvariantCulture);
            apiUrl = Environment.GetEnvironmentVariable("github_url");
            myGithub = Environment.GetEnvironmentVariable("github_uname");
            CommitUrl = new List<RepositoryCommits>();
            appName = Environment.GetEnvironmentVariable("appName");
            ProjectNames = new List<string>();
        }

        public async Task Fetch()
        {
            _httpClient!.DefaultRequestHeaders.Add("Accept", "application/vnd.github+json");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            AppendDate(apiUrl);

            _httpClient.DefaultRequestHeaders.Add("User-Agent", appName);
                
            await using Stream st = await _httpClient.GetStreamAsync(apiUrlDate /* + "&page=1"*/);

            await GetRepositories(st);

            await GetRepositoryCommits();

        }

        public async Task GetRepositories(Stream s)
        {
            var repo = await JsonSerializer.DeserializeAsync<List<Repositories>>(s);
            foreach(var r in repo ?? Enumerable.Empty<Repositories>())
            {
                ProjectNames.Add(r.Name);
            }


        }

        public async Task GetRepositoryCommits()
        {
            apiUrl = Environment.GetEnvironmentVariable("github_baseCommitUrl");
            foreach (var pr in ProjectNames) {
               
                apiUrl = String.Format("{0}/{1}/{2}/commits?since={3}&before={4}", apiUrl, myGithub, pr, dateSince, dateBefore);
                await using Stream st = await _httpClient!.GetStreamAsync(apiUrl);
                var repoCommits = await JsonSerializer.DeserializeAsync<List<RepositoryCommits>>(st);


                CommitUrl.AddRange(repoCommits!);
                apiUrl = Environment.GetEnvironmentVariable("github_baseCommitUrl");
            }

            foreach(var r in CommitUrl)
            {
                Console.WriteLine(r.ProjectUrl);
            }




        }

        private void AppendDate(string url)
        {
            apiUrlDate = String.Format("{0}&since={1}&before={2}", url, dateSince, dateBefore);
        }
    }
}