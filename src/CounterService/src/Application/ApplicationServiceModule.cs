using System.Reflection;
using DistributedCounter.CounterService.Utilities.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace DistributedCounter.CounterService.Application;

public class ApplicationServiceModule : ServiceModule
{
    public override void Load(IServiceCollection services)
    {
        services.AddMediatR(opts =>
        {
            opts.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
    }
}