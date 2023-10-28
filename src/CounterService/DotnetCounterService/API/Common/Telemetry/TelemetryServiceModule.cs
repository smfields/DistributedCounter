using DistributedCounter.CounterService.Utilities.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace DistributedCounter.CounterService.API.Common.Telemetry;

public class TelemetryServiceModule(IConfiguration configuration) : ServiceModule
{
    public override void Load(IServiceCollection services)
    {
        var zipkinOptions = configuration.GetOptions<ZipkinOptions>();
        
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
                // tracing.AddSource("Microsoft.Orleans.Runtime");
                tracing.AddSource("Microsoft.Orleans.Application");

                if (zipkinOptions.Enabled)
                {
                    tracing.AddZipkinExporter(zipkin =>
                    {
                        zipkin.Endpoint = new Uri(zipkinOptions.Endpoint);
                    });
                }
            });
    }
}