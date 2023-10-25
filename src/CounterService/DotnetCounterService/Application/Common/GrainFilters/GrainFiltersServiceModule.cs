using DistributedCounter.CounterService.Utilities.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace DistributedCounter.CounterService.Application.Common.GrainFilters;

public class GrainFiltersServiceModule : ServiceModule
{
    public override void Load(IServiceCollection services)
    {
        services.AddSingleton<IIncomingGrainCallFilter, IncomingRequestContextFilter>();
        services.AddSingleton<IOutgoingGrainCallFilter, OutgoingRequestContextFilter>();
        services.AddSingleton<IIncomingGrainCallFilter, LoggingFilter>();
    }
}