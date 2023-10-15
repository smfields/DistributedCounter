using DistributedCounter.CounterService.Domain.CounterAggregate;
using MediatR;

namespace DistributedCounter.CounterService.Application.Counters.GetCounterValue;

public record GetCounterValueQuery(Guid CounterId) : IRequest<GetCounterValueResponse>;
public record GetCounterValueResponse(long CurrentValue);

public class GetCounterValueQueryHandler : IRequestHandler<GetCounterValueQuery, GetCounterValueResponse>
{
    private readonly ICounterRepository _counterRepository;

    public GetCounterValueQueryHandler(ICounterRepository counterRepository)
    {
        _counterRepository = counterRepository;
    }
    
    public async Task<GetCounterValueResponse> Handle(GetCounterValueQuery query, CancellationToken cancellationToken)
    {
        var counter = await _counterRepository.GetCounterAsync(query.CounterId);
        return new GetCounterValueResponse(counter.Value);
    }
}