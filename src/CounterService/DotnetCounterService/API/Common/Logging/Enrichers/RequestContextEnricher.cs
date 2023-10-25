using DistributedCounter.CounterService.Utilities;
using Serilog.Core;
using Serilog.Events;

namespace DistributedCounter.CounterService.API.Common.Logging.Enrichers;

public class RequestContextEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var correlationId = propertyFactory.CreateProperty(nameof(RequestContext.CorrelationId), RequestContext.CorrelationId);
        var requestSource = propertyFactory.CreateProperty(nameof(RequestContext.RequestSource), RequestContext.RequestSource);
        logEvent.AddPropertyIfAbsent(correlationId);
        logEvent.AddPropertyIfAbsent(requestSource);
    }
}