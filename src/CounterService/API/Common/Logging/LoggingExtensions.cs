using Serilog;

namespace DistributedCounter.CounterService.API.Common.Logging;

public static class LoggingExtensions
{
    public static void ConfigureLogging(this ConfigureHostBuilder host)
    {
        host.UseSerilog((ctx, services, logger) => {
            logger
                .Enrich.FromLogContext()
                .ReadFrom.Configuration(ctx.Configuration);
        });
    }
}