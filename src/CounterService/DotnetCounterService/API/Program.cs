using DistributedCounter.CounterService.API.GRPC.Services;
using DistributedCounter.CounterService.Application.Common;
using DistributedCounter.CounterService.Application.Common.Locking;
using DistributedCounter.CounterService.Application.Counters.CreateCounter;
using DistributedCounter.CounterService.Domain.Locking;
using DistributedCounter.CounterService.Domain.Persistence;
using Microsoft.EntityFrameworkCore;
using RedLockNet;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddGrpc(opts =>
{
    opts.EnableDetailedErrors = true;
});
builder.Services.AddGrpcReflection();
builder.Services.AddMediatR(opts =>
{
    opts.RegisterServicesFromAssemblyContaining<CreateCounterCommand>();
});
builder.Services.AddDbContext<ApplicationDbContext>(opts => 
    opts.UseNpgsql(builder.Configuration.GetConnectionString("Database"))
);
builder.Services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());
builder.Services.AddSingleton<IConnectionMultiplexer>(
    ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis"))
);
builder.Services.AddSingleton<IDistributedLockFactory>(sp =>
{
    var redisConnection = sp.GetRequiredService<IConnectionMultiplexer>();
    var redLockConnections = new List<RedLockMultiplexer>
    {
        new(redisConnection)
    };

    return RedLockNet.SERedis.RedLockFactory.Create(redLockConnections);
});
builder.Services.AddScoped<ILockFactory, RedLockFactory>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<CounterService>();
app.MapGrpcReflectionService();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await dbContext.Database.EnsureCreatedAsync();

}
app.Run();
