using DistributedCounter.CounterService.Utilities;
using Serilog.Core;
using Serilog.Events;

namespace DistributedCounter.CounterService.API.Common.Logging.Enrichers;

public class RequestContextEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var traceId = propertyFactory.CreateProperty(nameof(RequestContext.TraceId), RequestContext.TraceId);
        var requestSource = propertyFactory.CreateProperty(nameof(RequestContext.RequestSource), RequestContext.RequestSource);
        logEvent.AddPropertyIfAbsent(traceId);
        logEvent.AddPropertyIfAbsent(requestSource);
    }
}