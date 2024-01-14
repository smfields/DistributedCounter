using DistributedCounter.CounterService.Utilities.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DistributedCounter.CounterService.Infrastructure.Orleans;

public class OrleansServiceModule(IConfiguration configuration, IHostEnvironment environment) : ServiceModule
{
    public override void Load(IServiceCollection services)
    {
        services.AddOrleans(siloBuilder =>
        {
            siloBuilder
                .AddActivityPropagation()
                .AddMemoryGrainStorageAsDefault();

            if (environment.IsDevelopment())
            {
                siloBuilder.UseLocalhostClustering();
            }
            else
            {
                
            }
        });
    }
}