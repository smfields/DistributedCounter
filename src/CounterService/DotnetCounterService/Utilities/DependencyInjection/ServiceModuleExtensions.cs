using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace DistributedCounter.CounterService.Utilities.DependencyInjection;

public static class ServiceModuleExtensions
{
    public static void RegisterAssemblyModules(
        this IHostApplicationBuilder host, 
        Assembly assembly,
        Action<IServiceCollection>? serviceModuleServices = null
    )
    {
        // Hold services including ServiceModules and dependencies they may require
        var serviceCollection = new ServiceCollection();
        
        // Add service module dependencies
        serviceCollection.AddSingleton<IConfiguration>(host.Configuration);
        serviceCollection.AddSingleton(host.Environment);
        serviceModuleServices?.Invoke(serviceCollection);

        // Scan for all service modules and register with DI container
        assembly
            .GetTypes()
            .Where(type => type.IsAssignableTo(typeof(ServiceModule)))
            .ToList()
            .ForEach(type => serviceCollection.AddSingleton(typeof(ServiceModule), type));
        
        // Retrieve registered service modules
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var serviceModules = serviceProvider.GetRequiredService<IEnumerable<ServiceModule>>();

        // Load services from each module
        foreach (var module in serviceModules)
        {
            module.Load(host.Services);
        }
        
        // Cleanup all service modules added
        host.Services.RemoveAll(typeof(ServiceModule));
    }
}