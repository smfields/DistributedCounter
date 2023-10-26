using DistributedCounter.CounterService.Utilities.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DistributedCounter.CounterService.Domain.Counters;

public class CounterServiceModule(IConfiguration configuration) : ServiceModule
{
    public override void Load(IServiceCollection services)
    {
        services.RegisterOptions<CounterOptions>(configuration);
    }
}