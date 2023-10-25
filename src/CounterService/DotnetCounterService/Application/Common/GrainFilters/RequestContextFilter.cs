using Orleans.Runtime;

namespace DistributedCounter.CounterService.Application.Common.GrainFilters;

public class IncomingRequestContextFilter : IIncomingGrainCallFilter
{
    public async Task Invoke(IIncomingGrainCallContext context)
    {
        var requestSource = RequestContext.Get(nameof(Utilities.RequestContext.RequestSource)) as string;
        
        Utilities.RequestContext.Initialize(requestSource);

        await context.Invoke();
    }
}

public class OutgoingRequestContextFilter : IOutgoingGrainCallFilter
{
    public async Task Invoke(IOutgoingGrainCallContext context)
    {
        RequestContext.Set(nameof(Utilities.RequestContext.RequestSource), Utilities.RequestContext.RequestSource);
        
        await context.Invoke();
    }
}