using DistributedCounter.CounterService.Utilities.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace DistributedCounter.CounterService.Domain.CounterAggregate;

public class CounterAggregateServiceModule : ServiceModule
{
    public override void Load(IServiceCollection services)
    {
        services.AddScoped<CounterClient.Factory>();
    }
}