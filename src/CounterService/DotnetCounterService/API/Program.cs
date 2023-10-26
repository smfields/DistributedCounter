using DistributedCounter.CounterService.API.Common.Logging;
using DistributedCounter.CounterService.API.Common.Orleans;
using DistributedCounter.CounterService.API.Counters.Services;
using DistributedCounter.CounterService.Utilities.DependencyInjection;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureLogging();
builder.ConfigureOrleans();
builder.Services.RegisterFromServiceModules(servicesAvailableToModules: services =>
{
    services.AddSingleton<IConfiguration>(builder.Configuration);
    services.AddSingleton(builder.Environment);
});

var app = builder.Build();

app.MapGrpcService<CounterService>();
app.MapGrpcReflectionService();

app.Run();
