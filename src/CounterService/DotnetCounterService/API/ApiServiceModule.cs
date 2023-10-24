using DistributedCounter.CounterService.Utilities.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace DistributedCounter.CounterService.API;

public class ApiServiceModule : ServiceModule
{
    public override void Load(IServiceCollection services)
    {
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

                tracing.AddZipkinExporter(zipkin =>
                {
                    zipkin.Endpoint = new Uri("http://localhost:9411/api/v2/spans");
                });
            });
    }
}