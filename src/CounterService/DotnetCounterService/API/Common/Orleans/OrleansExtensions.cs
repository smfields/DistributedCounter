namespace DistributedCounter.CounterService.API.Common.Orleans;

public static class OrleansExtensions
{
    public static void ConfigureOrleans(this ConfigureHostBuilder host)
    {
        host.UseOrleans(siloBuilder =>
        {
            siloBuilder
                .AddActivityPropagation()
                .UseLocalhostClustering()
                .AddMemoryGrainStorageAsDefault();
                // .AddCosmosGrainStorageAsDefault(configureOptions: options =>
                // {
                //     options.DatabaseName = "Orleans";
                //     options.DatabaseThroughput = 1000;
                //     options.ContainerName = "OrleansStorage";
                //     options.ConfigureCosmosClient(builder.Configuration.GetConnectionString("Database"));
                //     options.ClientOptions = new CosmosClientOptions()
                //     {
                //         HttpClientFactory = () => new HttpClient(new HttpClientHandler()
                //         {
                //             ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                //         }),
                //         ConnectionMode = ConnectionMode.Gateway,
                //         LimitToEndpoint = true
                //     };
                // });
        });
    }
}