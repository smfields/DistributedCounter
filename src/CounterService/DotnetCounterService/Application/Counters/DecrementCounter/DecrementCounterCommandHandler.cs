using DistributedCounter.CounterService.Domain.CounterAggregate;
using MediatR;

namespace DistributedCounter.CounterService.Application.Counters.DecrementCounter;

public record DecrementCounterCommand(Guid CounterId, uint Amount) : IRequest<DecrementCounterResponse>;

public record DecrementCounterResponse
{
    public static readonly DecrementCounterResponse Empty = new();
}

public class DecrementCounterCommandHandler(CounterClient.Factory counterClientFactory) : IRequestHandler<DecrementCounterCommand, DecrementCounterResponse>
{
    public async Task<DecrementCounterResponse> Handle(DecrementCounterCommand command, CancellationToken cancellationToken)
    {
        var counterClient = counterClientFactory.CreateClientFor(command.CounterId);
        await counterClient.Decrement(command.Amount);
        return DecrementCounterResponse.Empty;
    }
}