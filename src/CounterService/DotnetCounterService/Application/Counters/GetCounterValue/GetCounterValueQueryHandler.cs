using DistributedCounter.CounterService.Application.Common;
using MediatR;

namespace DistributedCounter.CounterService.Application.Counters.GetCounterValue;

public record GetCounterValueQuery(Guid CounterId) : IRequest<GetCounterValueResponse>;
public record GetCounterValueResponse(long CurrentValue);

public class GetCounterValueQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetCounterValueQuery, GetCounterValueResponse>
{
    public async Task<GetCounterValueResponse> Handle(GetCounterValueQuery query, CancellationToken cancellationToken)
    {
        var counter = await dbContext.Counters.FindAsync(new object?[]
        {
            query.CounterId
        }, cancellationToken: cancellationToken);

        if (counter is null)
        {
            throw new KeyNotFoundException("Counter: " + query.CounterId);
        }
        
        return new GetCounterValueResponse(counter.Value);
    }
}