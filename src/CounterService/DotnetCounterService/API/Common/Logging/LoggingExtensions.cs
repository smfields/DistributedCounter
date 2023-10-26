using DistributedCounter.CounterService.API.Common.Logging.Enrichers;
using Serilog;

namespace DistributedCounter.CounterService.API.Common.Logging;

public static class LoggingExtensions
{
    public static void ConfigureLogging(this ConfigureHostBuilder host)
    {
        host.UseSerilog((ctx, services, logger) => {
            logger
                .Enrich.FromLogContext()
                .Enrich.With(services.GetRequiredService<RequestContextEnricher>())
                // .WriteTo.Console()
                .ReadFrom.Configuration(ctx.Configuration);
        });
    }
}