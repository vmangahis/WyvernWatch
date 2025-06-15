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
using WyvernWatch.Utilities;

namespace WyvernWatch.Services.APIClient
{
    public class APIClient : IAPIClient
    {
        private HttpClient? _httpClient;
        private string? _apiKey;
        private string dateSince;
        private string dateBefore;
        private string? apiUrl;
        private List<string> ProjectNames;
        private List<RepositoryCommits> CommitUrl;
        private StringBuilder CommitSummary;
        private string? apiUrlDate;
        private string? appName;
        private string? _githubBaseCommitUrl;
        private string? myGithub;
        private Commit? cm;

        public APIClient(HttpClient httpClient)
        {
            _apiKey = Environment.GetEnvironmentVariable("github_publicTk");
            _httpClient = httpClient;
            _githubBaseCommitUrl = Environment.GetEnvironmentVariable("github_baseCommitUrl");
            dateBefore = DateTime.Now.Date.AddDays(1).ToString("s", CultureInfo.InvariantCulture);
            dateSince = DateTime.Now.Date.AddDays(-1).ToString("s", CultureInfo.InvariantCulture);
            apiUrl = Environment.GetEnvironmentVariable("github_url");
            myGithub = Environment.GetEnvironmentVariable("github_uname");
            CommitUrl = new List<RepositoryCommits>();
            appName = Environment.GetEnvironmentVariable("appName");
            ProjectNames = new List<string>();
            CommitSummary = new StringBuilder("Here is your summary today:\n");
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
            apiUrl = _githubBaseCommitUrl;
            foreach (var pr in ProjectNames) {
               
                apiUrl = String.Format("{0}/{1}/{2}/commits?since={3}&before={4}", apiUrl, myGithub, pr, dateSince, dateBefore);
                using Stream st = await _httpClient!.GetStreamAsync(apiUrl);
                var repoCommits = await JsonSerializer.DeserializeAsync<List<RepositoryCommits>>(st);


                CommitUrl.AddRange(repoCommits!);
                apiUrl = _githubBaseCommitUrl;
            }


            await GetRepositoryChanges();


        }

        public async Task GetRepositoryChanges() {
            string projectName = "";
            foreach (var c in CommitUrl) {
                if (projectName != UrlUtility.GetProjectName(c.ProjectUrl))
                {
                    projectName = UrlUtility.GetProjectName(c.ProjectUrl);
                    CommitSummary.AppendLine(projectName);
                }

                using Stream st = await _httpClient!.GetStreamAsync(c.ProjectUrl);
                var repoChanges = await JsonSerializer.DeserializeAsync<Commit>(st);
                repoChanges!.fc.ForEach(f => {
                    CommitSummary.AppendLine(String.Format("{0} - {1}", f.File, f.FileStatus));               
                } );
            }
        }


        // this is just a temp function, probably will just be included within the implementations above
        private void AppendDate(string url)
        {      
            apiUrlDate = String.Format("{0}&since={1}&before={2}", url, dateSince, dateBefore);
        }
    }
}