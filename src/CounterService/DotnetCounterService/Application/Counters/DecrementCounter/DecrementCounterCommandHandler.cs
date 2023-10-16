using DistributedCounter.CounterService.Application.Common;
using DistributedCounter.CounterService.Domain.CounterAggregate;
using MediatR;

namespace DistributedCounter.CounterService.Application.Counters.DecrementCounter;

public record DecrementCounterCommand(Guid CounterId, uint Amount) : IRequest<DecrementCounterResponse>;

public record DecrementCounterResponse
{
    public static DecrementCounterResponse Empty = new();
}

public class DecrementCounterCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<DecrementCounterCommand, DecrementCounterResponse>
{
    public async Task<DecrementCounterResponse> Handle(DecrementCounterCommand command, CancellationToken cancellationToken)
    {
        var counter = await dbContext.Counters.FindAsync(new object?[]
        {
            command.CounterId
        }, cancellationToken: cancellationToken);

        if (counter is null)
        {
            throw new KeyNotFoundException("Counter: " + command.CounterId);
        }
        
        counter.Decrement(command.Amount);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return DecrementCounterResponse.Empty;
    }
}