using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        // Register BlobServiceClient using the connection string from configuration
        services.AddSingleton(new BlobServiceClient(Environment.GetEnvironmentVariable("AzureWebJobsStorage")));
    })
    .Build();

host.Run();
