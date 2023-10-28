using DistributedCounter.CounterService.Domain.Persistence;
using DistributedCounter.CounterService.Utilities.DependencyInjection;
using Microsoft.Azure.Cosmos;

namespace DistributedCounter.CounterService.API.Common.Orleans;

public class OrleansServiceModule(IConfiguration configuration, IWebHostEnvironment environment) : ServiceModule
{
    public override void Load(IServiceCollection services)
    {
        var databaseOptions = configuration.GetOptions<DatabaseOptions>();
        
        services.AddOrleans(siloBuilder =>
        {
            siloBuilder
                .AddActivityPropagation()
                .AddMemoryGrainStorageAsDefault();
                // .AddCosmosGrainStorageAsDefault(configureOptions: options =>
                // {
                //     options.DatabaseName = "Orleans";
                //     options.DatabaseThroughput = 1000;
                //     options.ContainerName = "OrleansStorage";
                //     options.ConfigureCosmosClient(databaseOptions.ConnectionString);
                //
                //     if (environment.IsDevelopment())
                //     {
                //         options.ClientOptions = new CosmosClientOptions()
                //         {
                //             HttpClientFactory = () => new HttpClient(new HttpClientHandler()
                //             {
                //                 ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                //             }),
                //             ConnectionMode = ConnectionMode.Gateway,
                //             LimitToEndpoint = true
                //         };
                //     }
                // });

            if (environment.IsDevelopment())
            {
                siloBuilder.UseLocalhostClustering();
            }
            else
            {
                siloBuilder.UseCosmosClustering(configureOptions: options =>
                {
                    options.DatabaseName = "Orleans";
                    options.DatabaseThroughput = 1000;
                    options.ContainerName = "OrleansCluster";
                    options.ConfigureCosmosClient(databaseOptions.ConnectionString);
                });
            }
        });
    }
}