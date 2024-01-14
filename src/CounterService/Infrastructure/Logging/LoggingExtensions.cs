using Microsoft.AspNetCore.Builder;
using Serilog;

namespace DistributedCounter.CounterService.Domain.Logging;

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