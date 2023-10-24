using DistributedCounter.CounterService.Utilities.Configuration;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.Configuration;

public static class ConfigurationExtensions
{
    public static TOptions GetOptions<TOptions>(this IConfiguration configuration)
        where TOptions : class, IOptions, new()
    {
        var options = new TOptions();
        var section = configuration.GetSection(TOptions.Section);
        section.Bind(options);
        return options;
    }
}