using DistributedCounter.CounterService.API.Common.Logging.Enrichers;
using DistributedCounter.CounterService.Utilities.DependencyInjection;

namespace DistributedCounter.CounterService.API.Common.Logging;

public class LoggingServiceModule : ServiceModule
{
    public override void Load(IServiceCollection services)
    {
        services.AddSingleton<RequestContextEnricher>();
    }
}