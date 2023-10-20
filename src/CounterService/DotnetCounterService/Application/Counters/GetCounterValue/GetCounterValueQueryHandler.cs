using DistributedCounter.CounterService.Domain.CounterAggregate;
using MediatR;

namespace DistributedCounter.CounterService.Application.Counters.GetCounterValue;

public record GetCounterValueQuery(Guid CounterId) : IRequest<GetCounterValueResponse>;
public record GetCounterValueResponse(long CurrentValue);

public class GetCounterValueQueryHandler(IGrainFactory grainFactory) : IRequestHandler<GetCounterValueQuery, GetCounterValueResponse>
{
    public async Task<GetCounterValueResponse> Handle(GetCounterValueQuery query, CancellationToken cancellationToken)
    {
        var counter = grainFactory.GetGrain<ICounter>(query.CounterId);
        var currentValue = await counter.GetCurrentValue();
        return new GetCounterValueResponse(currentValue);
    }
}