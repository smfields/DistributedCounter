using DistributedCounter.CounterService.Domain.CounterAggregate;
using MediatR;

namespace DistributedCounter.CounterService.Application.Counters.IncrementCounter;

public record IncrementCounterCommand(Guid CounterId, uint Amount) : IRequest<IncrementCounterResponse>;

public record IncrementCounterResponse
{
    public static readonly IncrementCounterResponse Empty = new();
}

public class IncrementCounterCommandHandler(IGrainFactory grainFactory) : IRequestHandler<IncrementCounterCommand, IncrementCounterResponse>
{
    public async Task<IncrementCounterResponse> Handle(IncrementCounterCommand command, CancellationToken cancellationToken)
    {
        var counter = grainFactory.GetGrain<ICounter>(command.CounterId);
        await counter.Increment(command.Amount);
        return IncrementCounterResponse.Empty;
    }
}