using DistributedCounter.CounterService.Domain.CounterAggregate;
using MediatR;

namespace DistributedCounter.CounterService.Application.Counters.IncrementCounter;

public record IncrementCounterCommand(Guid CounterId, uint Amount) : IRequest<IncrementCounterResponse>;

public record IncrementCounterResponse
{
    public static IncrementCounterResponse Empty = new();
}

public class IncrementCounterCommandHandler : IRequestHandler<IncrementCounterCommand, IncrementCounterResponse>
{
    private readonly ICounterRepository _counterRepository;

    public IncrementCounterCommandHandler(ICounterRepository counterRepository)
    {
        _counterRepository = counterRepository;
    }
    
    public async Task<IncrementCounterResponse> Handle(IncrementCounterCommand command, CancellationToken cancellationToken)
    {
        var counter = await _counterRepository.GetCounterAsync(command.CounterId);
        counter.Increment(command.Amount);
        await _counterRepository.UpdateAsync(counter);
        return IncrementCounterResponse.Empty;
    }
}