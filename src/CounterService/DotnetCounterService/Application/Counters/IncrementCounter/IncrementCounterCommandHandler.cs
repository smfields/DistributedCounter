using DistributedCounter.CounterService.Domain.CounterAggregate;
using MediatR;

namespace DistributedCounter.CounterService.Application.Counters.IncrementCounter;

public record IncrementCounterCommand(Guid CounterId, uint Amount) : IRequest<IncrementCounterResponse>;

public record IncrementCounterResponse
{
    public static readonly IncrementCounterResponse Empty = new();
}

public class IncrementCounterCommandHandler(CounterClient.Factory counterClientFactory) : IRequestHandler<IncrementCounterCommand, IncrementCounterResponse>
{
    public async Task<IncrementCounterResponse> Handle(IncrementCounterCommand command, CancellationToken cancellationToken)
    {
        var counterClient = counterClientFactory.CreateClientFor(command.CounterId);
        await counterClient.Increment(command.Amount);
        return IncrementCounterResponse.Empty;
    }
}