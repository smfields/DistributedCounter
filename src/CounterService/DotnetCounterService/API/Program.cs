using DistributedCounter.CounterService.API.GRPC.Services;
using DistributedCounter.CounterService.Domain.Persistence;
using DistributedCounter.CounterService.Utilities.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterFromServiceModules(servicesAvailableToModules: services =>
{
    services.AddSingleton<IConfiguration>(builder.Configuration);
    services.AddSingleton(builder.Environment);
});

var app = builder.Build();

app.MapGrpcService<CounterService>();
app.MapGrpcReflectionService();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await dbContext.Database.EnsureCreatedAsync();
}

app.Run();
