using DistributedCounter.CounterService.Utilities.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DistributedCounter.CounterService.Utilities.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static TOptions RegisterAndGetOptions<TOptions>(this IServiceCollection services, IConfiguration configuration)
        where TOptions : class, IOptions, new()
    {
        services.RegisterOptions<TOptions>(configuration);
        return configuration.GetOptions<TOptions>();
    }

    public static void RegisterOptions<TOptions>(this IServiceCollection services, IConfiguration configuration)
        where TOptions : class, IOptions, new()
    {
        services.AddOptionsWithValidateOnStart<TOptions>().Bind(configuration.GetSection(TOptions.Section));
    }
}