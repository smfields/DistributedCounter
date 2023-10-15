using DistributedCounter.CounterService.Domain.CounterAggregate;
using MediatR;

namespace DistributedCounter.CounterService.Application.Counters.DecrementCounter;

public record DecrementCounterCommand(Guid CounterId, uint Amount) : IRequest<DecrementCounterResponse>;

public record DecrementCounterResponse
{
    public static DecrementCounterResponse Empty = new();
}

public class DecrementCounterCommandHandler : IRequestHandler<DecrementCounterCommand, DecrementCounterResponse>
{
    private readonly ICounterRepository _counterRepository;

    public DecrementCounterCommandHandler(ICounterRepository counterRepository)
    {
        _counterRepository = counterRepository;
    }
    
    public async Task<DecrementCounterResponse> Handle(DecrementCounterCommand command, CancellationToken cancellationToken)
    {
        var counter = await _counterRepository.GetCounterAsync(command.CounterId);
        counter.Decrement(command.Amount);
        await _counterRepository.UpdateAsync(counter);
        return DecrementCounterResponse.Empty;
    }
}