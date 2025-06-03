using System.Net.Http.Headers;
using System.Text.Json;
using WyvernWatch.Models;
using Microsoft.Extensions.Configuration;

ConfigurationBuilder conf = new ConfigurationBuilder();
IConfiguration iconf = conf.AddUserSecrets<Program>().Build();
string tk = iconf.GetSection("github")["publicTk"];
Console.WriteLine(tk);


using HttpClient client = new();
client.DefaultRequestHeaders.Accept.Clear();

client.DefaultRequestHeaders.Accept.Add( new MediaTypeWithQualityHeaderValue("application/vnd.github+json"));

client.DefaultRequestHeaders.Add("X-Github-Api-Version",  "2022-11-28");

//client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tk);

client.DefaultRequestHeaders.Add("Authorization", String.Format("Bearer {0}", tk));

client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("WyvernWatch", "1.0"));




await ProcessRepositoriesAsync(client);


static async Task ProcessRepositoriesAsync(HttpClient client)
{
    await using Stream stream =
    await client.GetStreamAsync("https://api.github.com/user/repos?affiliation=owner&sort=pushed&direction=desc");
    var repositories =
        await JsonSerializer.DeserializeAsync<List<Repositories>>(stream);
    
    foreach(var repo in repositories ?? Enumerable.Empty<Repositories>())
        Console.WriteLine(repo.Name);
}