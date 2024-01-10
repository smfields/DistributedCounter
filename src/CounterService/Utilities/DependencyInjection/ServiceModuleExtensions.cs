using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DistributedCounter.CounterService.Utilities.DependencyInjection;

public static class ServiceModuleExtensions
{
    public static void RegisterFromServiceModules(
        this IServiceCollection services,
        IEnumerable<Assembly> assemblies,
        Action<IServiceCollection>? servicesAvailableToModules = null
    )
    {
        // Hold services including ServiceModules and dependencies they may require
        var serviceCollection = new ServiceCollection();
        
        // Add service module dependencies
        servicesAvailableToModules?.Invoke(serviceCollection);

        // Scan for all service modules and register with DI container
        assemblies
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsAssignableTo(typeof(ServiceModule)) && !type.IsAbstract)
            .ToList()
            .ForEach(type => serviceCollection.AddSingleton(typeof(ServiceModule), type));
        
        // Retrieve registered service modules
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var serviceModules = serviceProvider.GetRequiredService<IEnumerable<ServiceModule>>();

        // Load services from each module
        foreach (var module in serviceModules)
        {
            module.Load(services);
        }
        
        // Cleanup all service modules added
        services.RemoveAll(typeof(ServiceModule));
    }
}