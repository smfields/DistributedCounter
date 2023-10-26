using DistributedCounter.CounterService.Domain.Counters;
using MediatR;

namespace DistributedCounter.CounterService.Application.Counters.DecrementCounter;

public record DecrementCounterCommand(Guid CounterId, uint Amount) : IRequest<DecrementCounterResponse>;

public record DecrementCounterResponse
{
    public static readonly DecrementCounterResponse Empty = new();
}

public class DecrementCounterCommandHandler(IGrainFactory grainFactory) : IRequestHandler<DecrementCounterCommand, DecrementCounterResponse>
{
    public async Task<DecrementCounterResponse> Handle(DecrementCounterCommand command, CancellationToken cancellationToken)
    {
        var counter = grainFactory.GetGrain<ICounter>(command.CounterId);
        await counter.Decrement(command.Amount);
        return DecrementCounterResponse.Empty;
    }
}