using WyvernWatch.Services.APIClient;
using WyvernWatch.Services.MailClient;

var mc = new MailClient();
var api = new APIClient();

await api.Fetch();

//mc.StartEmail();
