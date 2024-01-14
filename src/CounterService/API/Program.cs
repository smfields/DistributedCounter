using DistributedCounter.CounterService.API;
using DistributedCounter.CounterService.API.Counters.Services;
using DistributedCounter.CounterService.Application;
using DistributedCounter.CounterService.Domain;
using DistributedCounter.CounterService.Infrastructure;
using DistributedCounter.CounterService.Infrastructure.Logging;
using DistributedCounter.CounterService.Utilities;
using DistributedCounter.CounterService.Utilities.DependencyInjection;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureLogging();
builder.Services.RegisterFromServiceModules(
    new []
    {
        typeof(DomainServiceModule).Assembly,
        typeof(ApplicationServiceModule).Assembly,
        typeof(InfrastructureServiceModule).Assembly,
        typeof(ApiServiceModule).Assembly,
        typeof(UtilitiesServiceModule).Assembly
    },
    servicesAvailableToModules: services =>
    {
        services.AddSingleton<IConfiguration>(builder.Configuration);
        services.AddSingleton(builder.Environment);
        services.AddSingleton<IHostEnvironment>(sp => sp.GetRequiredService<IWebHostEnvironment>());
    }
);

var app = builder.Build();

app.MapGrpcService<CounterService>();
app.MapGrpcReflectionService();

app.Run();
