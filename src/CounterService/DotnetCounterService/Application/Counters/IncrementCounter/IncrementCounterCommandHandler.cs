using DistributedCounter.CounterService.Application.Common;
using DistributedCounter.CounterService.Application.Common.Locking;
using MediatR;

namespace DistributedCounter.CounterService.Application.Counters.IncrementCounter;

public record IncrementCounterCommand(Guid CounterId, uint Amount) : IRequest<IncrementCounterResponse>;

public record IncrementCounterResponse
{
    public static readonly IncrementCounterResponse Empty = new();
}

public class IncrementCounterCommandHandler(IApplicationDbContext dbContext, ILockFactory lockFactory) : IRequestHandler<IncrementCounterCommand, IncrementCounterResponse>
{
    public async Task<IncrementCounterResponse> Handle(IncrementCounterCommand command, CancellationToken cancellationToken)
    {
        await using var counterLock = await lockFactory.CreateLockAsync(command.CounterId.ToString(), cancellationToken: cancellationToken);
        
        var counter = await dbContext.Counters.FindAsync(new object?[]
        {
            command.CounterId
        }, cancellationToken: cancellationToken);

        if (counter is null)
        {
            throw new KeyNotFoundException("Counter: " + command.CounterId);
        }
        
        counter.Increment(command.Amount);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return IncrementCounterResponse.Empty;
    }
}