using System.Net.Http.Headers;
using System.Text.Json;
using WyvernWatch.Models;
using Microsoft.Extensions.Configuration;

ConfigurationBuilder conf = new ConfigurationBuilder();
IConfiguration iconf = conf.AddUserSecrets<Program>().Build();
string tk = iconf.GetSection("github")["publicTk"];
Console.WriteLine(tk);


/*using HttpClient client = new();
client.DefaultRequestHeaders.Accept.Clear();
client.DefaultRequestHeaders.Accept.Add( new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

await ProcessRepositoriesAsync(client);*/


static async Task ProcessRepositoriesAsync(HttpClient client)
{
    await using Stream stream =
    await client.GetStreamAsync("https://api.github.com/orgs/dotnet/repos");
    var repositories =
        await JsonSerializer.DeserializeAsync<List<Repositories>>(stream);
    
    foreach(var repo in repositories ?? Enumerable.Empty<Repositories>())
        Console.WriteLine(repo.Name);
}