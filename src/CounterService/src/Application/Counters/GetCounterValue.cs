using DistributedCounter.CounterService.Domain.Counters;
using MediatR;

namespace DistributedCounter.CounterService.Application.Counters;

public static class GetCounterValue
{
    public record Query(Guid CounterId) : IRequest<Response>;
    
    public record Response(long CurrentValue);
    
    internal sealed class Handler(IGrainFactory grainFactory) : IRequestHandler<Query, Response>
    {
        public async Task<Response> Handle(Query query, CancellationToken cancellationToken)
        {
            var counter = grainFactory.GetGrain<ICounter>(query.CounterId);
            var result = await counter.GetCurrentValue();
            return new Response(result.Value);
        }
    }

}