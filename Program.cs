using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WyvernWatch.Interfaces;
using WyvernWatch.Services.APIClient;
using WyvernWatch.Services.MailService;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

builder.Services.AddSingleton<ICourierService, CourierService>();

builder.Services.AddHttpClient<IAPIClient, APIClient>();



builder.Build().Run();
