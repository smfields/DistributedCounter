using DistributedCounter.CounterService.Domain.CounterAggregate;
using MediatR;

namespace DistributedCounter.CounterService.Application.Counters.GetCounterValue;

public record GetCounterValueQuery(Guid CounterId) : IRequest<GetCounterValueResponse>;
public record GetCounterValueResponse(long CurrentValue);

public class GetCounterValueQueryHandler(IGrainFactory grainFactory) : IRequestHandler<GetCounterValueQuery, GetCounterValueResponse>
{
    public async Task<GetCounterValueResponse> Handle(GetCounterValueQuery query, CancellationToken cancellationToken)
    {
        var counterClient = new CounterClient(query.CounterId, grainFactory);
        var currentValue = await counterClient.GetCurrentValue();
        return new GetCounterValueResponse(currentValue);
    }
}