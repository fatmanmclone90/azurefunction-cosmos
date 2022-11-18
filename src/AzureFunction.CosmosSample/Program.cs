using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System.IO;
using System.Threading.Tasks;

namespace AzureFunction.CosmosSample
{
    public static class Program
    {
        public static async Task Main()
        {
            var host = new HostBuilder().ConfigureFunctionsWorkerDefaults().ConfigureAppConfiguration(
                builder =>
                {
                    builder.SetBasePath(Directory.GetCurrentDirectory());
                    builder.AddJsonFile("appsettings.json");
                    var settings = builder.Build();

                    Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(settings).CreateLogger();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddLogging(builder => builder.ClearProviders().AddSerilog(Log.Logger));

                    var cosmosOptions = new CosmosOptions();
                    hostContext.Configuration.GetSection(nameof(CosmosOptions)).Bind(cosmosOptions);

                    services.AddOptions<CosmosOptions>()
                        .BindConfiguration(nameof(CosmosOptions))
                        .ValidateDataAnnotations()
                        .ValidateOnStart();

                    services.AddSingleton(s => new CosmosClient(
                        cosmosOptions.ConnectionString, 
                        new CosmosClientOptions { AllowBulkExecution = true }));
                    services.AddSingleton<ICosmosRepository, CosmosRepository>();

                }).Build();

            await host.RunAsync();
        }
    }
}
