namespace DistributedCounter.CounterService.Utilities;

public class RequestContext
{
    private static readonly AsyncLocal<ContextProperties> ContextData = new();

    public static string? CorrelationId => ContextData.Value?.CorrelationId;
    public static string? RequestSource => ContextData.Value?.RequestSource;

    public static void Initialize(
        string? correlationId,
        string? requestSource
    )
    {
        ContextData.Value = new ContextProperties(
            correlationId,
            requestSource
        );
    }

    public record ContextProperties(
        string? CorrelationId, 
        string? RequestSource
    );
}