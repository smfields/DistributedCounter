using DistributedCounter.CounterService.Application.Common.Locking;
using DistributedCounter.CounterService.Utilities.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RedLockNet;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;

namespace DistributedCounter.CounterService.Domain.Locking;

public class LockingServiceModule(IConfiguration configuration) : ServiceModule
{
    public override void Load(IServiceCollection services)
    {
        services.AddSingleton<IConnectionMultiplexer>(
            ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis"))
        );
        
        services.AddSingleton<IDistributedLockFactory>(sp =>
        {
            var redisConnection = sp.GetRequiredService<IConnectionMultiplexer>();
            var redLockConnections = new List<RedLockMultiplexer>
            {
                new(redisConnection)
            };

            return RedLockNet.SERedis.RedLockFactory.Create(redLockConnections);
        });
        
        services.AddScoped<ILockFactory, RedLockFactory>();
    }
}