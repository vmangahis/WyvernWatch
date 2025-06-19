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
using WyvernWatch.Models.API;
using WyvernWatch.Shared.DTO;
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
        private string? apiUrlDate;
        private string? appName;
        private string? _githubBaseCommitUrl;
        private string? myGithub;
        private string FinalSummary;

        public APIClient(HttpClient httpClient)
        {
            _apiKey = Environment.GetEnvironmentVariable("github_publicTk");
            _httpClient = httpClient;
            _githubBaseCommitUrl = Environment.GetEnvironmentVariable("github_baseCommitUrl");
            dateBefore = DateTime.Now.Date.AddHours(16).ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
            dateSince = DateTime.Now.Date.AddDays(-1).AddHours(16).ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
            apiUrl = Environment.GetEnvironmentVariable("github_url");
            myGithub = Environment.GetEnvironmentVariable("github_uname");
            appName = Environment.GetEnvironmentVariable("appName");
            FinalSummary = "";
        }

        public async Task<string> FetchAsync()
        {
            string url = $"{apiUrl}&since={dateSince}&before={dateBefore}";
            var headerRequest = new HttpRequestMessage(HttpMethod.Get, url);
            headerRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github"));
            headerRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            headerRequest.Headers.UserAgent.ParseAdd(appName);

            try
            {

                using var res = await _httpClient!.SendAsync(headerRequest);
                res.EnsureSuccessStatusCode();
                using Stream st = await res.Content.ReadAsStreamAsync();
                await GetRepositoriesAsync(st);


            }
            catch (HttpRequestException httpex)
            {
                Console.WriteLine(httpex);

            }


            return FinalSummary;


        }

        private async Task GetRepositoriesAsync(Stream s)
        {
            List<string> ProjectNames = new List<string>();


            var repo = await JsonSerializer.DeserializeAsync<List<Repositories>>(s);
            foreach(var r in repo ?? Enumerable.Empty<Repositories>())
            {
                ProjectNames.Add(r.Name);
            }

            await GetRepositoryCommitsAsync(ProjectNames);


        }

        private async Task GetRepositoryCommitsAsync(List<string> ProjectNames)
        {
            List<RepositoryCommits> CommitUrl = new List<RepositoryCommits>();
            apiUrl = _githubBaseCommitUrl;
            foreach (var pr in ProjectNames) {
                string url = $"{_githubBaseCommitUrl}/{myGithub}/{pr}/commits?since={dateSince}&before={dateBefore}";

                try {
                    var headers = new HttpRequestMessage(HttpMethod.Get, url);
                    headers.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github"));
                    headers.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
                    headers.Headers.UserAgent.ParseAdd(appName);
                    using var res = await _httpClient!.SendAsync(headers);
                    res.EnsureSuccessStatusCode();
                    using Stream st = res.Content.ReadAsStream();
                    var repoCommits = await JsonSerializer.DeserializeAsync<List<RepositoryCommits>>(st);


                    CommitUrl.AddRange(repoCommits!);
                    url = _githubBaseCommitUrl!;

                }
                catch (HttpRequestException ex) {
                    Console.WriteLine(ex);
                }

               
            }
            await GetRepositoryChangesAsync(CommitUrl);


        }

        private async Task GetRepositoryChangesAsync(List<RepositoryCommits> CommitUrl) {
            string projectName = "";
            StringBuilder CommitSummary = new StringBuilder("Here is your summary today:\n");
            foreach (var c in CommitUrl) {
                if (projectName != UrlUtility.GetProjectName(c.ProjectUrl))
                {
                    projectName = UrlUtility.GetProjectName(c.ProjectUrl);
                    CommitSummary.AppendLine(projectName);
                }

                try {
                    var headers = new HttpRequestMessage(HttpMethod.Get, c.ProjectUrl);
                    headers.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github"));
                    headers.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
                    headers.Headers.UserAgent.ParseAdd(appName);
                    using var res = await _httpClient!.SendAsync(headers);
                    res.EnsureSuccessStatusCode();
                    using Stream st = await res.Content.ReadAsStreamAsync();
                    var repoChanges = await JsonSerializer.DeserializeAsync<Commit>(st);
                    repoChanges!.fc.ForEach(f => {
                        CommitSummary.AppendLine(String.Format("{0} - {1}", f.File, f.FileStatus));
                    });

                }
                catch (HttpRequestException ex) {
                    Console.WriteLine(ex);
                }
                

            }

            GetSummary(CommitSummary.ToString());
        }

        private void GetSummary(string CommitSummary)
        {
            FinalSummary = CommitSummary;
        }
    }
}