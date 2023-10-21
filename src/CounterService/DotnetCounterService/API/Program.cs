using DistributedCounter.CounterService.API.GRPC.Services;
using DistributedCounter.CounterService.Application.Counters.CreateCounter;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseOrleans(siloBuilder =>
{
    siloBuilder
        .UseLocalhostClustering()
        // .AddMemoryGrainStorageAsDefault();
    .AddCosmosGrainStorageAsDefault(configureOptions: options =>
    {
        options.DatabaseName = "Orleans";
        options.DatabaseThroughput = 1000;
        options.ContainerName = "OrleansStorage";
        options.ConfigureCosmosClient(builder.Configuration.GetConnectionString("Database"));
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
