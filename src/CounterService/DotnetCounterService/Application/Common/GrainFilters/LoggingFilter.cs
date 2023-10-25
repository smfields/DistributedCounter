using Serilog.Context;

namespace DistributedCounter.CounterService.Application.Common.GrainFilters;

public class LoggingFilter : IIncomingGrainCallFilter
{
    public async Task Invoke(IIncomingGrainCallContext context)
    {
        using (LogContext.PushProperty("SourceGrainId", context.SourceId))
        using (LogContext.PushProperty("GrainId", context.TargetId))
        {
            await context.Invoke();
        }
    }
}


