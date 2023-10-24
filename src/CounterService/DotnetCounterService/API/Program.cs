using DistributedCounter.CounterService.API.GRPC.Services;
using DistributedCounter.CounterService.Utilities.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseOrleans(siloBuilder =>
{
    siloBuilder
        .UseLocalhostClustering()
        .AddMemoryGrainStorageAsDefault();
        // .AddCosmosGrainStorageAsDefault(configureOptions: options =>
        // {
        //     options.DatabaseName = "Orleans";
        //     options.DatabaseThroughput = 1000;
        //     options.ContainerName = "OrleansStorage";
        //     options.ConfigureCosmosClient(builder.Configuration.GetConnectionString("Database"));
        // });
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
