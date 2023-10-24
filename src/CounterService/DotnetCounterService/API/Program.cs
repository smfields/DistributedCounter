using DistributedCounter.CounterService.API.GRPC.Services;
using DistributedCounter.CounterService.Utilities.DependencyInjection;
using Microsoft.Azure.Cosmos;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseOrleans(siloBuilder =>
{
    siloBuilder
        .AddActivityPropagation()
        .UseLocalhostClustering()
        // .AddMemoryGrainStorageAsDefault();
        .AddCosmosGrainStorageAsDefault(configureOptions: options =>
        {
            options.DatabaseName = "Orleans";
            options.DatabaseThroughput = 1000;
            options.ContainerName = "OrleansStorage";
            options.ConfigureCosmosClient(builder.Configuration.GetConnectionString("Database"));
            options.ClientOptions = new CosmosClientOptions()
            {
                HttpClientFactory = () => new HttpClient(new HttpClientHandler()
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                }),
                ConnectionMode = ConnectionMode.Gateway,
                LimitToEndpoint = true
            };
        });
});

builder.Services.RegisterFromServiceModules(servicesAvailableToModules: services =>
{
    services.AddSingleton<IConfiguration>(builder.Configuration);
    services.AddSingleton(builder.Environment);
});

var app = builder.Build();

app.MapGrpcService<CounterService>();
app.MapGrpcReflectionService();

app.Run();
