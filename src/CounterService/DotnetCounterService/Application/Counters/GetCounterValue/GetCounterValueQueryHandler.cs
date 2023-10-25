using DistributedCounter.CounterService.Domain.CounterAggregate;
using MediatR;

namespace DistributedCounter.CounterService.Application.Counters.GetCounterValue;

public record GetCounterValueQuery(Guid CounterId) : IRequest<GetCounterValueResponse>;
public record GetCounterValueResponse(long CurrentValue);

public class GetCounterValueQueryHandler(CounterClient.Factory counterClientFactory) : IRequestHandler<GetCounterValueQuery, GetCounterValueResponse>
{
    public async Task<GetCounterValueResponse> Handle(GetCounterValueQuery query, CancellationToken cancellationToken)
    {
        var counterClient = counterClientFactory.CreateClientFor(query.CounterId);
        var currentValue = await counterClient.GetCurrentValue();
        return new GetCounterValueResponse(currentValue);
    }
}