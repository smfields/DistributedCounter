using OpenTelemetry.Trace;

namespace DistributedCounter.CounterService.Utilities;

public class RequestContext
{
    private static readonly AsyncLocal<ContextProperties> ContextData = new();

    public static string? TraceId => Tracer.CurrentSpan.Context.TraceId.ToString();
    public static string? RequestSource => ContextData.Value?.RequestSource;

    public static void Initialize(
        string? requestSource
    )
    {
        ContextData.Value = new ContextProperties(
            requestSource
        );
    }

    public record ContextProperties(
        string? RequestSource
    );
}