using DistributedCounter.CounterService.Utilities.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace DistributedCounter.CounterService.Infrastructure.Telemetry;

public class TelemetryServiceModule(IConfiguration configuration) : ServiceModule
{
    public override void Load(IServiceCollection services)
    {
        var zipkinOptions = configuration.GetOptions<ZipkinOptions>();
        
        AppContext.SetSwitch("Azure.Experimental.EnableActivitySource", true);
        
        services
            .AddOpenTelemetry()
            .WithTracing(tracing =>
            {
                tracing.SetResourceBuilder(
                    ResourceBuilder
                        .CreateDefault()
                        .AddService(serviceName: "CounterService-Dotnet", serviceVersion: "1.0.0")
                );
        
                tracing.AddAspNetCoreInstrumentation();
                tracing.AddSource("Microsoft.Orleans.Runtime");
                tracing.AddSource("Microsoft.Orleans.Application");
                tracing.AddSource("Azure.Cosmos.Operation");
                
                if (zipkinOptions.Enabled)
                {
                    tracing.AddZipkinExporter(zipkin =>
                    {
                        zipkin.Endpoint = new Uri(zipkinOptions.Endpoint);
                        zipkin.BatchExportProcessorOptions.MaxQueueSize = 4096 * 10;
                        zipkin.BatchExportProcessorOptions.ScheduledDelayMilliseconds = 2500;
                    });
                }
            });
    }
}