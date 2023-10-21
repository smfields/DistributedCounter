using DistributedCounter.CounterService.API.GRPC.Services;
using DistributedCounter.CounterService.Application.Counters.CreateCounter;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseOrleans(siloBuilder =>
{
    siloBuilder
        .UseLocalhostClustering()
        .AddCosmosGrainStorageAsDefault(configureOptions: static options =>
        {
            options.DatabaseName = "Orleans";
            options.DatabaseThroughput = 1000;
            options.ContainerName = "OrleansStorage";
            options.ConfigureCosmosClient("AccountEndpoint=https://distributed-counter-db.documents.azure.com:443/;AccountKey=fFndWiRuX9idm4n4sQxiHcHX26T3F3iBqxb5zzmSN7qxHyITFgrhjGwqE1qAq9WWAMeLjmsEQuOyACDbbNRHew==;");
        });

});
builder.Services.AddGrpc(opts =>
{
    opts.EnableDetailedErrors = true;
});
builder.Services.AddGrpcReflection();
builder.Services.AddMediatR(opts =>
{
    opts.RegisterServicesFromAssemblyContaining<CreateCounterCommand>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<CounterService>();
app.MapGrpcReflectionService();

app.Run();
