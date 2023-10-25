using DistributedCounter.CounterService.Utilities.DependencyInjection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace DistributedCounter.CounterService.Application.Common.Behaviors;

public class BehaviorServiceModule : ServiceModule
{
    public override void Load(IServiceCollection services)
    {
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
    }
}