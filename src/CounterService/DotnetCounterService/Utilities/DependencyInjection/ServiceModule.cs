using Microsoft.Extensions.DependencyInjection;

namespace DistributedCounter.CounterService.Utilities.DependencyInjection;

public abstract class ServiceModule
{
    public abstract void Load(IServiceCollection services);
}